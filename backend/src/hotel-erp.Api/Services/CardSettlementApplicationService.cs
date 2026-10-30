using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Payments;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record CardSettlementCreationResult(
    CardSettlement? Settlement,
    int? FailureStatus,
    string? FailureDetail)
{
    public bool Succeeded => Settlement is not null;

    public static CardSettlementCreationResult Created(CardSettlement settlement) => new(settlement, null, null);
    public static CardSettlementCreationResult BadRequest(string detail) => new(null, StatusCodes.Status400BadRequest, detail);
    public static CardSettlementCreationResult Conflict(string detail) => new(null, StatusCodes.Status409Conflict, detail);
}

public sealed class CardSettlementApplicationService(
    ApplicationDbContext context,
    IAccountingService accountingService,
    IAuditService auditService,
    IdempotencyService idempotencyService,
    IHotelClock clock,
    IApplicationFailureInjector failureInjector,
    OperationalMutationGuard access)
{
    public async Task<CardSettlementCreationResult> CreateAsync(
        SecurityActor actor,
        CreateCardSettlementRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        var userId = actor.Id;
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);

        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
            return CardSettlementCreationResult.BadRequest("Idempotency-Key debe ser un UUID válido");
        if (request.SettlementDate == default)
            return CardSettlementCreationResult.BadRequest("La fecha de liquidación es obligatoria");
        if (request.SettlementDate > clock.Today)
            return CardSettlementCreationResult.BadRequest("La fecha de liquidación no puede estar en el futuro");

        var grossAmount = TaxService.RoundCurrency(request.GrossAmount);
        var bankDepositAmount = TaxService.RoundCurrency(request.BankDepositAmount);
        var commissionAmount = TaxService.RoundCurrency(request.CommissionAmount);
        var withholdingAmount = TaxService.RoundCurrency(request.WithholdingAmount);
        var applications = request.Applications
            .Select(application => new NormalizedSettlementApplication(
                application.PaymentId,
                TaxService.RoundCurrency(application.Amount)))
            .OrderBy(application => application.PaymentId)
            .ToList();
        if (grossAmount <= 0m || applications.Any(application => application.Amount <= 0m))
            return CardSettlementCreationResult.BadRequest("Los importes de la liquidación deben ser mayores que cero");
        if (applications.Select(application => application.PaymentId).Distinct().Count() != applications.Count)
            return CardSettlementCreationResult.BadRequest("Un pago solo puede aparecer una vez dentro de la liquidación");
        if (applications.Sum(application => application.Amount) != grossAmount)
            return CardSettlementCreationResult.BadRequest("La suma de pagos aplicados debe coincidir exactamente con el importe bruto");
        if (bankDepositAmount + commissionAmount + withholdingAmount != grossAmount)
            return CardSettlementCreationResult.BadRequest("Depósito, comisión y retención deben sumar exactamente el importe bruto");

        var externalReference = request.ExternalReference.Trim().ToUpperInvariant();
        if (externalReference.Length < 3)
            return CardSettlementCreationResult.BadRequest("La liquidación requiere una referencia de al menos 3 caracteres");

        var requestHash = IdempotencyService.ComputeRequestHash(new
        {
            request.Currency,
            GrossAmount = grossAmount,
            BankDepositAmount = bankDepositAmount,
            CommissionAmount = commissionAmount,
            WithholdingAmount = withholdingAmount,
            ExternalReference = externalReference,
            request.SettlementDate,
            Applications = applications
        });

        try
        {
            var existingIntent = await idempotencyService.LockAndFindAsync(
                userId,
                "card-settlement:create",
                normalizedKey,
                requestHash,
                cancellationToken);
            if (existingIntent is not null)
            {
                var existingSettlement = await DetailedSettlements()
                    .AsSplitQuery()
                    .SingleOrDefaultAsync(
                        settlement => settlement.Id == existingIntent.ResourceId,
                        cancellationToken);
                if (existingSettlement is null)
                    return CardSettlementCreationResult.Conflict("El resultado idempotente de la liquidación ya no está disponible");
                await transaction.CommitAsync(cancellationToken);
                return CardSettlementCreationResult.Created(existingSettlement);
            }
        }
        catch (IdempotencyConflictException exception)
        {
            return CardSettlementCreationResult.Conflict(exception.Message);
        }

        var referenceLock = $"card-settlement-reference:{externalReference}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({referenceLock}));",
            cancellationToken);
        if (await context.CardSettlements.AnyAsync(
                settlement => settlement.ExternalReference == externalReference,
                cancellationToken))
        {
            return CardSettlementCreationResult.Conflict("La referencia del adquirente ya fue registrada");
        }

        foreach (var paymentId in applications.Select(application => application.PaymentId))
        {
            var paymentLock = $"payment-financial:{paymentId:N}";
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({paymentLock}));",
                cancellationToken);
        }

        var paymentIds = applications.Select(application => application.PaymentId).ToList();
        var payments = await context.Payments
            .Include(payment => payment.Refunds)
            .Include(payment => payment.CardSettlementApplications)
            .ThenInclude(application => application.CardSettlement)
            .Where(payment => paymentIds.Contains(payment.Id))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        if (payments.Count != paymentIds.Count)
            return CardSettlementCreationResult.BadRequest("Uno o más pagos no existen");

        foreach (var application in applications)
        {
            var payment = payments.Single(candidate => candidate.Id == application.PaymentId);
            if (payment.Method != PaymentMethod.Tarjeta || payment.Status == PaymentStatus.Anulado)
                return CardSettlementCreationResult.Conflict($"El pago {payment.PaymentNumber} no es un cobro con tarjeta liquidable");
            var refunded = payment.Refunds
                .Where(refund => refund.Status == RefundStatus.Confirmado)
                .Sum(refund => refund.Amount);
            var settled = payment.CardSettlementApplications
                .Where(existing => existing.CardSettlement.Status == CardSettlementStatus.Confirmado)
                .Sum(existing => existing.Amount);
            var available = Math.Max(0m, TaxService.RoundCurrency(payment.Amount - refunded - settled));
            if (application.Amount > available)
                return CardSettlementCreationResult.Conflict($"La aplicación excede el saldo por liquidar de {payment.PaymentNumber}");
        }

        var settlementId = Guid.CreateVersion7();
        var recordedBy = await context.Users.SingleAsync(user => user.Id == userId, cancellationToken);
        var settlement = new CardSettlement
        {
            Id = settlementId,
            SettlementNumber = $"LTJ-{settlementId:N}".ToUpperInvariant(),
            RecordedByUserId = userId,
            RecordedByUser = recordedBy,
            Currency = request.Currency,
            GrossAmount = grossAmount,
            BankDepositAmount = bankDepositAmount,
            CommissionAmount = commissionAmount,
            WithholdingAmount = withholdingAmount,
            ExternalReference = externalReference,
            SettlementDate = request.SettlementDate,
            Status = CardSettlementStatus.Confirmado,
            Applications = applications.Select(application => new CardSettlementApplication
            {
                Id = Guid.CreateVersion7(),
                PaymentId = application.PaymentId,
                Payment = payments.Single(payment => payment.Id == application.PaymentId),
                Amount = application.Amount
            }).ToList()
        };
        context.CardSettlements.Add(settlement);
        await context.SaveChangesAsync(cancellationToken);
        await accountingService.CreateCardSettlementEntryAsync(settlement);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.CardSettlementAfterAccounting,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateCardSettlement",
            nameof(CardSettlement),
            settlement.Id,
            new
            {
                settlement.SettlementNumber,
                settlement.Currency,
                settlement.GrossAmount,
                settlement.BankDepositAmount,
                settlement.CommissionAmount,
                settlement.WithholdingAmount,
                settlement.ExternalReference,
                settlement.SettlementDate,
                Applications = applications
            });
        await idempotencyService.StoreAsync(
            userId,
            "card-settlement:create",
            normalizedKey,
            requestHash,
            settlement.Id,
            cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CardSettlementCreationResult.Created(settlement);
    }

    private IQueryable<CardSettlement> DetailedSettlements() => context.CardSettlements
        .Include(settlement => settlement.RecordedByUser)
        .Include(settlement => settlement.AccountingEntry)
        .Include(settlement => settlement.Applications)
        .ThenInclude(application => application.Payment);

    private sealed record NormalizedSettlementApplication(Guid PaymentId, decimal Amount);
}
