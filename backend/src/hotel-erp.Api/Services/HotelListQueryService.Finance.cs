using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Cash;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Dtos.Payments;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed partial class HotelListQueryService
{
    public async Task<PagedResult<CashRegisterDto>> CashRegistersAsync(CashRegisterPageQuery request, CancellationToken token)
    {
        var query = context.CashRegisters.AsNoTracking();
        if (request.OpenOnly)
            query = query.Where(register => register.IsActive && register.CashMovements
                .OrderByDescending(movement => movement.CreatedAt).ThenByDescending(movement => movement.Id)
                .Select(movement => movement.MovementType != CashMovementType.Cierre).FirstOrDefault());
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(register => register.Name.ToLower().Contains(term)
                || (register.Description != null && register.Description.ToLower().Contains(term)));
        }
        var page = await query.OrderBy(register => register.Name).ThenBy(register => register.Id).ToPageAsync(request, token);
        var ids = page.Items.Select(register => register.Id).ToList();
        var rows = await CashRegistersWithLatestMovement().Where(register => ids.Contains(register.Id)).ToListAsync(token);
        var lookup = rows.ToDictionary(register => register.Id);
        return new(ids.Where(lookup.ContainsKey).Select(id => CashRegisterApplicationService.ToDto(
            lookup[id], lookup[id].CashMovements.SingleOrDefault())).ToList(),
            page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public async Task<CashRegisterDto?> CashRegisterAsync(Guid id, CancellationToken token)
    {
        var register = await CashRegistersWithLatestMovement().SingleOrDefaultAsync(register => register.Id == id, token);
        return register is null ? null : CashRegisterApplicationService.ToDto(register, register.CashMovements.SingleOrDefault());
    }

    private IQueryable<CashRegister> CashRegistersWithLatestMovement() => context.CashRegisters.AsNoTracking()
        .Include(register => register.CashMovements.OrderByDescending(movement => movement.CreatedAt)
            .ThenByDescending(movement => movement.Id).Take(1));

    public async Task<PagedResult<CashMovementDto>> CashMovementsAsync(Guid registerId, CashMovementPageQuery request, CancellationToken token)
    {
        var query = context.CashMovements.AsNoTracking().Where(movement => movement.CashRegisterId == registerId);
        if (!string.IsNullOrWhiteSpace(request.MovementType))
        {
            var type = Enum.Parse<CashMovementType>(request.MovementType);
            query = query.Where(movement => movement.MovementType == type);
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(movement => (movement.Description != null && movement.Description.ToLower().Contains(term))
                || (movement.User.FirstName + " " + movement.User.LastName).ToLower().Contains(term));
        }
        var page = await query.OrderByDescending(movement => movement.CreatedAt).ThenByDescending(movement => movement.Id)
            .Include(movement => movement.User).Include(movement => movement.CashRegister)
            .OrderByDescending(movement => movement.CreatedAt).ThenByDescending(movement => movement.Id).ToPageAsync(request, token);
        return new(mapper.Map<List<CashMovementDto>>(page.Items), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public async Task<PagedResult<CardSettlementDto>> CardSettlementsAsync(SearchPageQuery request, CancellationToken token)
    {
        var query = context.CardSettlements.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(settlement => settlement.SettlementNumber.ToLower().Contains(term)
                || settlement.ExternalReference.ToLower().Contains(term));
        }
        var page = await query.OrderByDescending(settlement => settlement.SettlementDate).ThenByDescending(settlement => settlement.Id)
            .ToPageAsync(request, token);
        var ids = page.Items.Select(settlement => settlement.Id).ToList();
        var rows = await context.CardSettlements.AsNoTracking().Where(settlement => ids.Contains(settlement.Id))
            .Include(settlement => settlement.RecordedByUser).Include(settlement => settlement.AccountingEntry)
            .Include(settlement => settlement.Applications).ThenInclude(application => application.Payment)
            .AsSplitQuery().ToListAsync(token);
        var lookup = rows.ToDictionary(settlement => settlement.Id);
        return new(mapper.Map<List<CardSettlementDto>>(ids.Where(lookup.ContainsKey).Select(id => lookup[id])),
            page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    private IQueryable<EligibleCardPaymentDto> EligibleCardPayments() => context.Payments.AsNoTracking()
        .Where(payment => payment.Method == PaymentMethod.Tarjeta && payment.Status != PaymentStatus.Anulado)
        .Select(payment => new EligibleCardPaymentDto
        {
            Id = payment.Id,
            PaymentNumber = payment.PaymentNumber,
            PaymentDate = payment.PaymentDate,
            ExternalReference = payment.ExternalReference,
            Amount = payment.Amount,
            RefundedAmount = payment.Refunds.Where(refund => refund.Status == RefundStatus.Confirmado)
                .Sum(refund => (decimal?)refund.Amount) ?? 0m,
            SettledAmount = payment.CardSettlementApplications.Where(application => application.CardSettlement.Status == CardSettlementStatus.Confirmado)
                .Sum(application => (decimal?)application.Amount) ?? 0m,
            // All three persisted amounts have precision 18,2; subtraction is exact decimal arithmetic.
            AvailableAmount = payment.Amount
                - (payment.Refunds.Where(refund => refund.Status == RefundStatus.Confirmado).Sum(refund => (decimal?)refund.Amount) ?? 0m)
                - (payment.CardSettlementApplications.Where(application => application.CardSettlement.Status == CardSettlementStatus.Confirmado)
                    .Sum(application => (decimal?)application.Amount) ?? 0m)
        }).Where(payment => payment.AvailableAmount > 0m);

    public Task<PagedResult<EligibleCardPaymentDto>> EligibleCardPaymentsAsync(SearchPageQuery request, CancellationToken token)
    {
        var query = EligibleCardPayments();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(payment => payment.PaymentNumber.ToLower().Contains(term) || payment.ExternalReference.ToLower().Contains(term));
        }
        return query.OrderBy(payment => payment.PaymentDate).ThenBy(payment => payment.Id).ToPageAsync(request, token);
    }

    public async Task<EligibleCardSummaryDto> EligibleCardSummaryAsync(CancellationToken token)
    {
        var query = EligibleCardPayments();
        return new(await query.CountAsync(token), await query.SumAsync(payment => (decimal?)payment.AvailableAmount, token) ?? 0m);
    }

    public Task<PagedResult<AccountingEntryListDto>> JournalEntriesAsync(SearchPageQuery request, CancellationToken token)
    {
        var query = context.AccountingEntries.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            var types = Enum.GetValues<EntryType>().Where(type => type.ToString().Contains(term, StringComparison.OrdinalIgnoreCase)).ToArray();
            query = query.Where(entry => entry.Description.ToLower().Contains(term) || types.Contains(entry.EntryType)
                || entry.EntryItems.Any(item => item.Account.AccountName.ToLower().Contains(term) || item.Account.AccountNumber.Contains(term)));
        }
        return query.Select(entry => new AccountingEntryListDto
        {
            Id = entry.Id,
            TransactionDate = entry.TransactionDate,
            Description = entry.Description,
            EntryType = entry.EntryType.ToString(),
            TotalDebit = entry.EntryItems.Sum(item => (decimal?)item.Debit) ?? 0m,
            TotalCredit = entry.EntryItems.Sum(item => (decimal?)item.Credit) ?? 0m,
            ItemCount = entry.EntryItems.Count()
        }).OrderByDescending(entry => entry.TransactionDate).ThenByDescending(entry => entry.Id).ToPageAsync(request, token);
    }

    public async Task<JournalSummaryDto> JournalSummaryAsync(CancellationToken token)
    {
        var items = context.EntryItems.AsNoTracking().Where(item => !item.Entry.IsDeleted);
        var totals = await items.GroupBy(_ => 1).Select(group => new { Debit = group.Sum(item => item.Debit), Credit = group.Sum(item => item.Credit) })
            .SingleOrDefaultAsync(token);
        return new(totals?.Debit ?? 0m, totals?.Credit ?? 0m);
    }

    public Task<PagedResult<BackupLogDto>> BackupLogsAsync(BackupPageQuery request, CancellationToken token)
    {
        var query = context.BackupLogs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = Enum.Parse<BackupStatus>(request.Status);
            query = query.Where(log => log.Status == status);
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(log => log.FileName.ToLower().Contains(term));
        }
        return query.Select(log => new BackupLogDto
        {
            Id = log.Id,
            StartedAt = log.StartedAt,
            CompletedAt = log.CompletedAt,
            FileName = log.FileName,
            SizeBytes = log.SizeBytes,
            Sha256Hash = log.Sha256Hash,
            Status = log.Status.ToString(),
            UploadedAt = log.UploadedAt,
            UploadAttempts = log.UploadAttempts,
            ErrorMessage = log.ErrorMessage
        }).OrderByDescending(log => log.StartedAt).ThenByDescending(log => log.Id).ToPageAsync(request, token);
    }
}
