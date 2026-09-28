using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Payments;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record RefundCreationResult(Refund? Refund, int? FailureStatus, string? FailureDetail)
{
    public bool Succeeded => Refund is not null;

    public static RefundCreationResult Created(Refund refund) => new(refund, null, null);
    public static RefundCreationResult BadRequest(string detail) => new(null, StatusCodes.Status400BadRequest, detail);
    public static RefundCreationResult NotFound(string detail) => new(null, StatusCodes.Status404NotFound, detail);
    public static RefundCreationResult Conflict(string detail) => new(null, StatusCodes.Status409Conflict, detail);
}

public sealed class RefundApplicationService(
    ApplicationDbContext context,
    IAccountingService accountingService,
    AuditService auditService,
    IdempotencyService idempotencyService,
    IHotelClock clock,
    IApplicationFailureInjector failureInjector)
{
    public async Task<RefundCreationResult> CreateAsync(
        Guid userId,
        Guid paymentId,
        CreateRefundRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
            return RefundCreationResult.BadRequest("Idempotency-Key debe ser un UUID válido");

        var amount = TaxService.RoundCurrency(request.Amount);
        var applications = request.Applications
            .Select(application => new NormalizedRefundApplication(
                application.CreditNoteId,
                TaxService.RoundCurrency(application.Amount)))
            .OrderBy(application => application.CreditNoteId)
            .ToList();
        if (applications.Select(application => application.CreditNoteId).Distinct().Count() != applications.Count)
            return RefundCreationResult.BadRequest("Una nota de crédito solo puede aparecer una vez dentro del reembolso");
        if (applications.Sum(application => application.Amount) != amount)
            return RefundCreationResult.BadRequest("La suma de aplicaciones debe coincidir exactamente con el importe del reembolso");

        var reason = request.Reason.Trim();
        if (reason.Length < 3)
            return RefundCreationResult.BadRequest("El motivo del reembolso debe contener al menos 3 caracteres");
        var externalReference = request.ExternalReference?.Trim() ?? string.Empty;
        var requestHash = IdempotencyService.ComputeRequestHash(new
        {
            PaymentId = paymentId,
            Amount = amount,
            request.CashRegisterId,
            ExternalReference = externalReference,
            Reason = reason,
            Applications = applications
        });

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var existingIntent = await idempotencyService.LockAndFindAsync(
                userId,
                "payment:refund",
                normalizedKey,
                requestHash,
                cancellationToken);
            if (existingIntent is not null)
            {
                var existingRefund = await DetailedRefunds(paymentId)
                    .AsSplitQuery()
                    .SingleOrDefaultAsync(refund => refund.Id == existingIntent.ResourceId, cancellationToken);
                if (existingRefund is null)
                    return RefundCreationResult.Conflict("El resultado idempotente del reembolso ya no está disponible");
                await transaction.CommitAsync(cancellationToken);
                return RefundCreationResult.Created(existingRefund);
            }
        }
        catch (IdempotencyConflictException exception)
        {
            return RefundCreationResult.Conflict(exception.Message);
        }

        var paymentLock = $"payment-financial:{paymentId:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({paymentLock}));",
            cancellationToken);

        var requestedCreditNoteIds = applications.Select(application => application.CreditNoteId).ToList();
        var noteOrigins = await context.Invoices
            .Where(invoice => requestedCreditNoteIds.Contains(invoice.Id))
            .Select(invoice => new CreditNoteOrigin(invoice.Id, invoice.OriginalInvoiceId))
            .ToListAsync(cancellationToken);
        if (noteOrigins.Count != requestedCreditNoteIds.Count || noteOrigins.Any(note => !note.OriginalInvoiceId.HasValue))
            return RefundCreationResult.BadRequest("Una o más notas de crédito no existen o no tienen documento original");

        var invoiceIds = noteOrigins
            .Select(note => note.OriginalInvoiceId!.Value)
            .Distinct()
            .OrderBy(id => id)
            .ToList();
        foreach (var invoiceId in invoiceIds)
        {
            var balanceLock = $"invoice-balance:{invoiceId:N}";
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({balanceLock}));",
                cancellationToken);
        }

        var payment = await context.Payments
            .Include(candidate => candidate.Applications)
            .Include(candidate => candidate.Refunds)
            .Include(candidate => candidate.CardSettlementApplications)
            .ThenInclude(application => application.CardSettlement)
            .AsSplitQuery()
            .SingleOrDefaultAsync(candidate => candidate.Id == paymentId, cancellationToken);
        if (payment is null) return RefundCreationResult.NotFound("Pago no encontrado");
        if (payment.Status == PaymentStatus.Anulado)
            return RefundCreationResult.Conflict("El pago anulado no admite reembolsos");

        var previouslyRefundedFromPayment = payment.Refunds
            .Where(refund => refund.Status == RefundStatus.Confirmado)
            .Sum(refund => refund.Amount);
        if (amount > TaxService.RoundCurrency(payment.Amount - previouslyRefundedFromPayment))
            return RefundCreationResult.Conflict("El reembolso excede el importe disponible del pago original");
        if (payment.Method == PaymentMethod.Tarjeta)
        {
            var settledFromPayment = payment.CardSettlementApplications
                .Where(application => application.CardSettlement.Status == CardSettlementStatus.Confirmado)
                .Sum(application => application.Amount);
            var unsettledAmount = TaxService.RoundCurrency(
                payment.Amount - previouslyRefundedFromPayment - settledFromPayment);
            if (amount > unsettledAmount)
                return RefundCreationResult.Conflict("El pago con tarjeta ya fue liquidado; su devolución requiere el flujo de reversión del adquirente");
        }

        var creditNotes = await context.Invoices
            .Where(invoice => requestedCreditNoteIds.Contains(invoice.Id))
            .ToListAsync(cancellationToken);
        var paymentInvoiceIds = payment.Applications.Select(application => application.InvoiceId).ToHashSet();
        foreach (var application in applications)
        {
            var creditNote = creditNotes.Single(note => note.Id == application.CreditNoteId);
            if (creditNote.DocumentType != InvoiceDocumentType.NotaCredito
                || !creditNote.OriginalInvoiceId.HasValue
                || !paymentInvoiceIds.Contains(creditNote.OriginalInvoiceId.Value))
            {
                return RefundCreationResult.Conflict($"El documento {creditNote.CorrelativeNumber} no respalda un reembolso de este pago");
            }

            var alreadyAppliedToNote = await context.RefundApplications
                .Where(existing => existing.CreditNoteId == creditNote.Id
                    && existing.Refund.Status == RefundStatus.Confirmado)
                .SumAsync(existing => existing.Amount, cancellationToken);
            if (application.Amount > TaxService.RoundCurrency(creditNote.TotalAmount - alreadyAppliedToNote))
                return RefundCreationResult.Conflict($"El reembolso excede el saldo de la nota {creditNote.CorrelativeNumber}");
        }

        foreach (var invoiceId in invoiceIds)
        {
            var invoiceTotal = await context.Invoices
                .Where(invoice => invoice.Id == invoiceId)
                .Select(invoice => invoice.TotalAmount)
                .SingleAsync(cancellationToken);
            var paid = await context.PaymentApplications
                .Where(application => application.InvoiceId == invoiceId
                    && application.Payment.Status != PaymentStatus.Anulado)
                .SumAsync(application => application.Amount, cancellationToken);
            var credited = await context.Invoices
                .Where(invoice => invoice.OriginalInvoiceId == invoiceId
                    && invoice.DocumentType == InvoiceDocumentType.NotaCredito)
                .SumAsync(invoice => invoice.TotalAmount, cancellationToken);
            var refunded = await context.RefundApplications
                .Where(application => application.CreditNote.OriginalInvoiceId == invoiceId
                    && application.Refund.Status == RefundStatus.Confirmado)
                .SumAsync(application => application.Amount, cancellationToken);
            var requestedForInvoice = applications
                .Where(application => noteOrigins.Single(note => note.Id == application.CreditNoteId).OriginalInvoiceId == invoiceId)
                .Sum(application => application.Amount);
            var availableCustomerCredit = Math.Max(
                0m,
                TaxService.RoundCurrency(paid + credited - invoiceTotal - refunded));
            if (requestedForInvoice > availableCustomerCredit)
                return RefundCreationResult.Conflict("El reembolso excede el saldo a favor respaldado por pagos y notas de crédito");
        }

        CashRegister? cashRegister = null;
        decimal? cashBalanceAfter = null;
        if (payment.Method == PaymentMethod.Efectivo)
        {
            if (!request.CashRegisterId.HasValue)
                return RefundCreationResult.BadRequest("El reembolso en efectivo requiere una caja abierta");
            if (!string.IsNullOrEmpty(externalReference))
                return RefundCreationResult.BadRequest("El reembolso en efectivo no utiliza referencia bancaria");

            var cashRegisterId = request.CashRegisterId.Value;
            var cashLock = $"cash-register:{cashRegisterId:N}";
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({cashLock}));",
                cancellationToken);
            cashRegister = await context.CashRegisters
                .SingleOrDefaultAsync(
                    register => register.Id == cashRegisterId && register.IsActive,
                    cancellationToken);
            if (cashRegister is null)
                return RefundCreationResult.BadRequest("La caja seleccionada no existe o está inactiva");
            var lastMovement = await context.CashMovements
                .Where(movement => movement.CashRegisterId == cashRegisterId)
                .OrderByDescending(movement => movement.CreatedAt)
                .ThenByDescending(movement => movement.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (lastMovement is null || lastMovement.MovementType == CashMovementType.Cierre)
                return RefundCreationResult.Conflict("La caja seleccionada está cerrada");
            if (lastMovement.BalanceAfter < amount)
                return RefundCreationResult.Conflict("La caja no tiene saldo suficiente para efectuar el reembolso");
            cashBalanceAfter = TaxService.RoundCurrency(lastMovement.BalanceAfter - amount);
        }
        else
        {
            if (request.CashRegisterId.HasValue)
                return RefundCreationResult.BadRequest("Tarjeta y transferencia no deben afectar la caja de efectivo");
            if (externalReference.Length < 3)
                return RefundCreationResult.BadRequest("La devolución por tarjeta o transferencia requiere una referencia de al menos 3 caracteres");
        }

        var now = clock.HondurasNow;
        var refundId = Guid.CreateVersion7();
        var recordedBy = await context.Users.SingleAsync(user => user.Id == userId, cancellationToken);
        var refund = new Refund
        {
            Id = refundId,
            RefundNumber = $"REF-{refundId:N}".ToUpperInvariant(),
            PaymentId = payment.Id,
            Payment = payment,
            RecordedByUserId = userId,
            RecordedByUser = recordedBy,
            Method = payment.Method,
            Currency = "HNL",
            Amount = amount,
            ExternalReference = payment.Method == PaymentMethod.Efectivo ? string.Empty : externalReference,
            Reason = reason,
            RefundDate = now,
            Status = RefundStatus.Confirmado,
            CashRegisterId = payment.Method == PaymentMethod.Efectivo ? request.CashRegisterId : null,
            CashRegister = cashRegister,
            Applications = applications.Select(application => new RefundApplication
            {
                Id = Guid.CreateVersion7(),
                CreditNoteId = application.CreditNoteId,
                CreditNote = creditNotes.Single(note => note.Id == application.CreditNoteId),
                Amount = application.Amount
            }).ToList()
        };
        context.Refunds.Add(refund);

        if (payment.Method == PaymentMethod.Efectivo)
        {
            context.CashMovements.Add(new CashMovement
            {
                Id = Guid.CreateVersion7(),
                CashRegisterId = request.CashRegisterId!.Value,
                UserId = userId,
                MovementType = CashMovementType.Egreso,
                Amount = amount,
                Description = $"Reembolso {refund.RefundNumber}",
                MovementDate = now,
                BalanceAfter = cashBalanceAfter!.Value,
                ReferenceId = refund.Id
            });
        }

        var refundedAfter = TaxService.RoundCurrency(previouslyRefundedFromPayment + amount);
        payment.Status = refundedAfter == payment.Amount
            ? PaymentStatus.Reembolsado
            : PaymentStatus.ParcialmenteReembolsado;

        await context.SaveChangesAsync(cancellationToken);
        await accountingService.CreateRefundEntryAsync(refund);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.RefundAfterAccounting,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateRefund",
            nameof(Refund),
            refund.Id,
            new
            {
                refund.RefundNumber,
                refund.PaymentId,
                payment.PaymentNumber,
                refund.Method,
                refund.Amount,
                refund.CashRegisterId,
                refund.ExternalReference,
                refund.Reason,
                Applications = applications
            },
            paymentMethod: refund.Method.ToString());
        await idempotencyService.StoreAsync(
            userId,
            "payment:refund",
            normalizedKey,
            requestHash,
            refund.Id,
            cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return RefundCreationResult.Created(refund);
    }

    private IQueryable<Refund> DetailedRefunds(Guid paymentId) => context.Refunds
        .Where(refund => refund.PaymentId == paymentId)
        .Include(refund => refund.Payment)
        .Include(refund => refund.RecordedByUser)
        .Include(refund => refund.CashRegister)
        .Include(refund => refund.AccountingEntry)
        .Include(refund => refund.Applications)
        .ThenInclude(application => application.CreditNote);

    private sealed record NormalizedRefundApplication(Guid CreditNoteId, decimal Amount);
    private sealed record CreditNoteOrigin(Guid Id, Guid? OriginalInvoiceId);
}
