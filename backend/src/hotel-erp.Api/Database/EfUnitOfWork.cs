using hotel_erp.Api.Services.Interfaces;

namespace hotel_erp.Api.Database;

public sealed class EfUnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
