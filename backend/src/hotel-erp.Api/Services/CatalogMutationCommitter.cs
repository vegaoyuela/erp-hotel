using hotel_erp.Api.Database;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed class CatalogMutationCommitter(ApplicationDbContext context, IUnitOfWork unitOfWork,
    IAuditService audit, IApplicationFailureInjector failures)
{
    public async Task LockAsync(string catalog, CancellationToken token)
    {
        RequireTransaction();
        var key = $"hotel-catalog:{catalog}";
        await context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtext({key}));", token);
    }

    public async Task PersistAsync(SecurityActor actor, string operation, string entity, Func<Guid> targetId, CancellationToken token)
    {
        RequireTransaction();
        // Only schema field names are audited here; values may contain guest or fiscal private data.
        var fields = context.ChangeTracker.Entries()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(entry => new
            {
                Entity = entry.Metadata.ClrType.Name,
                Fields = entry.Properties.Where(property => entry.State != EntityState.Modified || property.IsModified)
                    .Select(property => property.Metadata.Name).Order().ToArray()
            }).ToArray();
        await unitOfWork.SaveChangesAsync(token);
        await failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterPersistence, token);
        if (fields.Length > 0)
            await audit.LogAsync(actor.Id, operation, entity, targetId(), new { ChangedFields = fields });
        await failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterAudit, token);
    }

    private void RequireTransaction()
    {
        if (context.Database.CurrentTransaction is null)
            throw new InvalidOperationException("La mutación de catálogo requiere una transacción exterior.");
    }
}
