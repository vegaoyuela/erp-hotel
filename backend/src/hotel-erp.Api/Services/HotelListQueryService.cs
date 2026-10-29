using AutoMapper;
using hotel_erp.Api.Controllers;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Dtos.Customer;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed partial class HotelListQueryService(ApplicationDbContext context, IMapper mapper)
{
    public async Task<PagedResult<GuestDto>> GuestsAsync(GuestPageQuery request, CancellationToken token)
    {
        var query = context.Guests.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(guest => (guest.FirstName + " " + guest.LastName).ToLower().Contains(term)
                || (guest.DocumentNumber != null && guest.DocumentNumber.ToLower().Contains(term))
                || (guest.Phone != null && guest.Phone.ToLower().Contains(term))
                || (guest.Company != null && guest.Company.ToLower().Contains(term)));
        }
        var page = await query.OrderBy(guest => guest.LastName).ThenBy(guest => guest.FirstName)
            .ThenBy(guest => guest.Id).ToPageAsync(request, token);
        return new(mapper.Map<List<GuestDto>>(page.Items), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public async Task<PagedResult<InvoiceDto>> InvoicesAsync(InvoicePageQuery request, CancellationToken token)
    {
        var query = context.Invoices.AsNoTracking();
        if (request.CaiId.HasValue) query = query.Where(invoice => invoice.CAIId == request.CaiId);
        if (request.DocumentAuthorizationId.HasValue)
            query = query.Where(invoice => invoice.DocumentAuthorizationId == request.DocumentAuthorizationId);
        if (request.OriginalInvoiceId.HasValue)
            query = query.Where(invoice => invoice.OriginalInvoiceId == request.OriginalInvoiceId);
        if (!string.IsNullOrWhiteSpace(request.Dni))
        {
            var document = request.Dni.Trim();
            query = query.Where(invoice => invoice.RTNCliente == document
                || (invoice.Guest != null && invoice.Guest.DocumentNumber == document));
        }
        if (request.Start.HasValue)
        {
            var start = request.Start.Value.ToDateTime(TimeOnly.MinValue);
            query = query.Where(invoice => invoice.InvoiceDate >= start);
        }
        if (request.End.HasValue && request.End.Value != DateOnly.MaxValue)
        {
            var endExclusive = request.End.Value.AddDays(1).ToDateTime(TimeOnly.MinValue);
            query = query.Where(invoice => invoice.InvoiceDate < endExclusive);
        }
        if (request.DocumentType is not null)
        {
            var type = Enum.Parse<InvoiceDocumentType>(request.DocumentType);
            query = query.Where(invoice => invoice.DocumentType == type);
        }
        // Page invoice identities first; related ledger rows are fetched only for the requested page.
        var page = await query.OrderByDescending(invoice => invoice.InvoiceDate)
            .ThenByDescending(invoice => invoice.Id).ToPageAsync(request, token);
        var ids = page.Items.Select(invoice => invoice.Id).ToList();
        var invoices = await context.Invoices.AsNoTracking().Where(invoice => ids.Contains(invoice.Id))
            .Include(invoice => invoice.InvoiceItems).Include(invoice => invoice.CAI)
            .Include(invoice => invoice.Customer).Include(invoice => invoice.Guest)
            .Include(invoice => invoice.PaymentApplications).ThenInclude(application => application.Payment)
            .Include(invoice => invoice.CreditNotes).AsSplitQuery().ToListAsync(token);
        var lookup = invoices.ToDictionary(invoice => invoice.Id);
        var ordered = ids.Where(lookup.ContainsKey).Select(id => lookup[id]);
        return new(mapper.Map<List<InvoiceDto>>(ordered), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public async Task<PagedResult<AuditLogDto>> AuditAsync(AuditPageQuery request, CancellationToken token)
    {
        var query = context.AuditLogs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Action))
            query = query.Where(log => log.Action == request.Action);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(log => log.Action.ToLower().Contains(term)
                || (log.EntityName != null && log.EntityName.ToLower().Contains(term))
                || (log.CorrelativeNumber != null && log.CorrelativeNumber.ToLower().Contains(term))
                || (log.Hash != null && log.Hash.ToLower().Contains(term))
                || (log.User != null && (log.User.Username.ToLower().Contains(term)
                    || (log.User.FirstName + " " + log.User.LastName).ToLower().Contains(term))));
        }
        var page = await query.OrderByDescending(log => log.Timestamp).ThenByDescending(log => log.Id)
            .Select(log => new AuditLogDto
            {
                Id = log.Id,
                UserId = log.UserId,
                UserName = log.User != null ? log.User.Username : null,
                UserFullName = log.User != null ? log.User.FirstName + " " + log.User.LastName : "Sistema",
                Action = log.Action,
                EntityName = log.EntityName,
                EntityId = log.EntityId,
                CorrelativeNumber = log.CorrelativeNumber,
                PaymentMethod = log.PaymentMethod,
                PreviousHash = log.PreviousHash,
                Hash = log.Hash,
                Changes = log.Changes,
                Timestamp = log.Timestamp,
                HondurasTimestamp = log.HondurasTimestamp
            }).OrderByDescending(log => log.Timestamp).ThenByDescending(log => log.Id).ToPageAsync(request, token);
        return page;
    }
}
