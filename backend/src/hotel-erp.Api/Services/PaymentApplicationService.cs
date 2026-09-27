using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Payments;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record PaymentCreationResult(Payment? Payment, int? FailureStatus, string? FailureDetail)
{
    public bool Succeeded => Payment is not null;

    public static PaymentCreationResult Created(Payment payment) => new(payment, null, null);
    public static PaymentCreationResult BadRequest(string detail) => new(null, StatusCodes.Status400BadRequest, detail);
    public static PaymentCreationResult Conflict(string detail) => new(null, StatusCodes.Status409Conflict, detail);
}

public sealed class PaymentApplicationService(
    ApplicationDbContext context,
    IAccountingService accountingService,
    AuditService auditService,
    IdempotencyService idempotencyService,
    IHotelClock clock)
{
    public async Task<PaymentCreationResult> CreateAsync(
        Guid userId,
        CreatePaymentRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
            return PaymentCreationResult.BadRequest("Idempotency-Key debe ser un UUID válido");
        if (!Enum.TryParse<PaymentMethod>(request.Method, out var method))
            return PaymentCreationResult.BadRequest("Método de pago no soportado");

        var amount = TaxService.RoundCurrency(request.Amount);
        var applications = request.Applications
            .Select(application => new NormalizedPaymentApplication(
                application.InvoiceId,
                TaxService.RoundCurrency(application.Amount)))
            .OrderBy(application => application.InvoiceId)
            .ToList();
        if (applications.Select(application => application.InvoiceId).Distinct().Count() != applications.Count)
            return PaymentCreationResult.BadRequest("Una factura solo puede aparecer una vez dentro del pago");
        if (applications.Sum(application => application.Amount) != amount)
            return PaymentCreationResult.BadRequest("La suma de aplicaciones debe coincidir exactamente con el importe del pago");

        var cashReceived = request.CashReceived.HasValue
            ? TaxService.RoundCurrency(request.CashReceived.Value)
            : (decimal?)null;
        var externalReference = request.ExternalReference?.Trim() ?? string.Empty;
        if (method == PaymentMethod.Efectivo)
        {
            if (!request.CashRegisterId.HasValue)
                return PaymentCreationResult.BadRequest("El pago en efectivo requiere una caja abierta");
            if (!cashReceived.HasValue || cashReceived.Value < amount)
                return PaymentCreationResult.BadRequest("El efectivo recibido debe cubrir el importe aplicado");
        }
        else
        {
            if (request.CashRegisterId.HasValue || cashReceived.HasValue)
                return PaymentCreationResult.BadRequest("Tarjeta y transferencia no deben afectar la caja de efectivo");
            if (externalReference.Length < 3)
                return PaymentCreationResult.BadRequest("Tarjeta y transferencia requieren una referencia de al menos 3 caracteres");
        }

        var requestHash = IdempotencyService.ComputeRequestHash(new
        {
            Method = method.ToString(),
            request.Currency,
            Amount = amount,
            CashReceived = cashReceived,
            request.CashRegisterId,
            ExternalReference = externalReference,
            Applications = applications
        });
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var existingIntent = await idempotencyService.LockAndFindAsync(
                userId,
                "payment:create",
                normalizedKey,
                requestHash,
                cancellationToken);
            if (existingIntent is not null)
            {
                var existingPayment = await DetailedPayments()
                    .AsSplitQuery()
                    .SingleOrDefaultAsync(payment => payment.Id == existingIntent.ResourceId, cancellationToken);
                if (existingPayment is null)
                    return PaymentCreationResult.Conflict("El resultado idempotente del pago ya no está disponible");
                await transaction.CommitAsync(cancellationToken);
                return PaymentCreationResult.Created(existingPayment);
            }
        }
        catch (IdempotencyConflictException exception)
        {
            return PaymentCreationResult.Conflict(exception.Message);
        }

        foreach (var invoiceId in applications.Select(application => application.InvoiceId))
        {
            var lockName = $"invoice-balance:{invoiceId:N}";
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({lockName}));",
                cancellationToken);
        }

        var invoiceIds = applications.Select(application => application.InvoiceId).ToList();
        var invoices = await context.Invoices
            .Include(invoice => invoice.PaymentApplications)
            .ThenInclude(application => application.Payment)
            .Include(invoice => invoice.CreditNotes)
            .Include(invoice => invoice.Folio)
            .ThenInclude(folio => folio!.Reservation)
            .Include(invoice => invoice.Folio)
            .ThenInclude(folio => folio!.Room)
            .Where(invoice => invoiceIds.Contains(invoice.Id))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        if (invoices.Count != invoiceIds.Count)
            return PaymentCreationResult.BadRequest("Una o más facturas no existen");

        foreach (var application in applications)
        {
            var invoice = invoices.Single(candidate => candidate.Id == application.InvoiceId);
            if (invoice.DocumentType == InvoiceDocumentType.NotaCredito || invoice.Status == InvoiceStatus.Anulada)
                return PaymentCreationResult.Conflict($"El documento {invoice.CorrelativeNumber} no admite pagos");
            var paid = invoice.PaymentApplications
                .Where(existing => existing.Payment.Status != PaymentStatus.Anulado)
                .Sum(existing => existing.Amount);
            var credited = invoice.CreditNotes
                .Where(note => note.DocumentType == InvoiceDocumentType.NotaCredito)
                .Sum(note => note.TotalAmount);
            if (application.Amount > TaxService.RoundCurrency(invoice.TotalAmount - paid - credited))
                return PaymentCreationResult.Conflict($"El pago excede el saldo de {invoice.CorrelativeNumber}");
        }

        decimal? cashBalanceAfter = null;
        if (method == PaymentMethod.Efectivo)
        {
            var cashRegisterId = request.CashRegisterId!.Value;
            var cashLock = $"cash-register:{cashRegisterId:N}";
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({cashLock}));",
                cancellationToken);
            var register = await context.CashRegisters
                .SingleOrDefaultAsync(
                    candidate => candidate.Id == cashRegisterId && candidate.IsActive,
                    cancellationToken);
            if (register is null)
                return PaymentCreationResult.BadRequest("La caja seleccionada no existe o está inactiva");
            var lastMovement = await context.CashMovements
                .Where(movement => movement.CashRegisterId == cashRegisterId)
                .OrderByDescending(movement => movement.CreatedAt)
                .ThenByDescending(movement => movement.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (lastMovement is null || lastMovement.MovementType == CashMovementType.Cierre)
                return PaymentCreationResult.Conflict("La caja seleccionada está cerrada");
            cashBalanceAfter = TaxService.RoundCurrency(lastMovement.BalanceAfter + amount);
        }

        var now = clock.HondurasNow;
        var paymentId = Guid.CreateVersion7();
        var recordedBy = await context.Users.SingleAsync(user => user.Id == userId, cancellationToken);
        var payment = new Payment
        {
            Id = paymentId,
            PaymentNumber = $"PAG-{paymentId:N}".ToUpperInvariant(),
            RecordedByUserId = userId,
            RecordedByUser = recordedBy,
            Method = method,
            Currency = request.Currency,
            Amount = amount,
            CashReceived = cashReceived,
            CashChange = method == PaymentMethod.Efectivo
                ? TaxService.RoundCurrency(cashReceived!.Value - amount)
                : null,
            ExternalReference = externalReference,
            PaymentDate = now,
            Status = PaymentStatus.Confirmado,
            CashRegisterId = request.CashRegisterId,
            Applications = applications.Select(application => new PaymentApplication
            {
                Id = Guid.CreateVersion7(),
                InvoiceId = application.InvoiceId,
                Invoice = invoices.Single(invoice => invoice.Id == application.InvoiceId),
                Amount = application.Amount
            }).ToList()
        };
        context.Payments.Add(payment);

        if (method == PaymentMethod.Efectivo)
        {
            context.CashMovements.Add(new CashMovement
            {
                Id = Guid.CreateVersion7(),
                CashRegisterId = request.CashRegisterId!.Value,
                UserId = userId,
                MovementType = CashMovementType.Ingreso,
                Amount = amount,
                Description = $"Cobro {payment.PaymentNumber}",
                MovementDate = now,
                BalanceAfter = cashBalanceAfter!.Value,
                ReferenceId = payment.Id
            });
        }

        foreach (var invoice in invoices)
        {
            var newApplication = applications.Single(application => application.InvoiceId == invoice.Id);
            var priorPaid = invoice.PaymentApplications
                .Where(existing => existing.PaymentId != payment.Id && existing.Payment.Status != PaymentStatus.Anulado)
                .Sum(existing => existing.Amount);
            var credited = invoice.CreditNotes
                .Where(note => note.DocumentType == InvoiceDocumentType.NotaCredito)
                .Sum(note => note.TotalAmount);
            var paidAfter = TaxService.RoundCurrency(priorPaid + newApplication.Amount);
            invoice.Status = paidAfter + credited == invoice.TotalAmount
                ? InvoiceStatus.Pagada
                : InvoiceStatus.Emitida;
            var methods = invoice.PaymentApplications
                .Where(existing => existing.PaymentId != payment.Id && existing.Payment.Status != PaymentStatus.Anulado)
                .Select(existing => existing.Payment.Method)
                .Append(method)
                .Distinct()
                .ToList();
            var paymentMethodSummary = methods.Count == 1 ? methods[0].ToString() : "Mixto";

            if (invoice.Status == InvoiceStatus.Pagada && invoice.Folio?.Status == FolioStatus.PendientePago)
            {
                invoice.Folio.Status = FolioStatus.Cerrado;
                invoice.Folio.ClosingDate = now;
                invoice.Folio.Reservation.Status = ReservationStatus.CheckOut;
                invoice.Folio.Reservation.Version++;
                invoice.Folio.Room.Status = RoomStatus.Limpieza;
                await auditService.LogAsync(
                    userId,
                    "CheckOut",
                    nameof(Reservation),
                    invoice.Folio.ReservationId,
                    new { invoice.FolioId, InvoiceId = invoice.Id, PaymentId = payment.Id, invoice.CorrelativeNumber },
                    invoice.CorrelativeNumber,
                    paymentMethodSummary);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        await accountingService.CreatePaymentEntryAsync(payment);
        await auditService.LogAsync(
            userId,
            "CreatePayment",
            nameof(Payment),
            payment.Id,
            new
            {
                payment.PaymentNumber,
                payment.Method,
                payment.Currency,
                payment.Amount,
                payment.CashRegisterId,
                payment.ExternalReference,
                Applications = applications
            },
            paymentMethod: payment.Method.ToString());
        await idempotencyService.StoreAsync(
            userId,
            "payment:create",
            normalizedKey,
            requestHash,
            payment.Id,
            cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return PaymentCreationResult.Created(payment);
    }

    private IQueryable<Payment> DetailedPayments() => context.Payments
        .Include(payment => payment.RecordedByUser)
        .Include(payment => payment.CashRegister)
        .Include(payment => payment.AccountingEntry)
        .Include(payment => payment.Applications)
        .ThenInclude(application => application.Invoice)
        .Include(payment => payment.Refunds)
        .ThenInclude(refund => refund.RecordedByUser)
        .Include(payment => payment.Refunds)
        .ThenInclude(refund => refund.CashRegister)
        .Include(payment => payment.Refunds)
        .ThenInclude(refund => refund.Applications)
        .ThenInclude(application => application.CreditNote)
        .Include(payment => payment.CardSettlementApplications)
        .ThenInclude(application => application.CardSettlement);

    private sealed record NormalizedPaymentApplication(Guid InvoiceId, decimal Amount);
}
