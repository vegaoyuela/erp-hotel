using System.Data;
using hotel_erp.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace hotel_erp.Api.Services;

public static class SecurityMutationLocks
{
    public const string Catalog = "hotel-security-catalog";

    public static string UserKey(Guid id) => $"hotel-security-user:{id:N}";

    // Order: catalog, user (when needed), business resources, then audit-chain.
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
            var userLock = UserKey(userId);
            await context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtext({userLock}));", token);
            return transaction;
        }
        catch { await transaction.DisposeAsync(); throw; }
    }
}
