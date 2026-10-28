namespace hotel_erp.Api.Services.Interfaces;

/// <summary>
/// Persists the changes staged by repositories in the current request scope.
/// When an application service owns a transaction, this flushes changes without
/// committing that transaction; the application service remains its owner.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
