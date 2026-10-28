using hotel_erp.Api.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public static class PagedQuery
{
    public static async Task<PagedResult<T>> ToPageAsync<T>(
        this IOrderedQueryable<T> query, PageQuery request, CancellationToken cancellationToken = default)
    {
        var count = await query.CountAsync(cancellationToken);
        var pages = Math.Max(1, (int)Math.Ceiling((decimal)count / request.PageSize));
        var page = Math.Min(request.Page, pages);
        var items = await query.Skip((page - 1) * request.PageSize).Take(request.PageSize)
            .ToListAsync(cancellationToken);
        return new(items, page, request.PageSize, count, pages);
    }
}
