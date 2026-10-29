using System.Data;
using hotel_erp.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace hotel_erp.Api.Services;

public static class SecurityMutationLocks
{
    public const string Catalog = "hotel-security-catalog";

    // Always acquire catalog, then user (for session writes), then audit-chain.
    public static async Task<IDbContextTransaction> BeginAdministrationAsync(ApplicationDbContext context, CancellationToken token)
    {
        var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, token);
        try
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtext({Catalog}));", token);
            return transaction;
        }
        catch { await transaction.DisposeAsync(); throw; }
    }

    public static async Task<IDbContextTransaction> BeginSessionAsync(ApplicationDbContext context, Guid userId, CancellationToken token = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, token);
        try
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock_shared(hashtext({Catalog}));", token);
            var userLock = $"hotel-security-user:{userId:N}";
            await context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtext({userLock}));", token);
            return transaction;
        }
        catch { await transaction.DisposeAsync(); throw; }
    }
}
