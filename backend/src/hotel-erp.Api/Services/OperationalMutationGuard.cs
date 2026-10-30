using System.Data;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace hotel_erp.Api.Services;

public sealed class OperationalAccessException(int status, string detail) : Exception(detail)
{
    public int Status { get; } = status;
}

public sealed class OperationalMutationGuard(ApplicationDbContext context, IApplicationFailureInjector failures)
{
    public async Task<IDbContextTransaction> BeginAsync(SecurityActor actor, string permission, CancellationToken token)
    {
        var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, token);
        try
        {
            // Keep both shared locks until the caller commits or disposes this business transaction.
            // Security writers acquire the same locks before revoking users, roles or sessions.
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock_shared(hashtext({SecurityMutationLocks.Catalog}));", token);
            var userLock = SecurityMutationLocks.UserKey(actor.Id);
            await context.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock_shared(hashtext({userLock}));", token);
            var user = await context.Users.AsNoTracking().Where(row => row.Id == actor.Id)
                .Select(row => new { row.IsActive, row.IsDeleted, row.MustChangePassword, row.SecurityVersion }).SingleOrDefaultAsync(token);
            if (user is null || !user.IsActive || user.IsDeleted || user.SecurityVersion != actor.Version)
                throw new OperationalAccessException(StatusCodes.Status401Unauthorized, "La sesión fue revocada; vuelva a iniciar sesión");
            var allowed = !user.MustChangePassword && await context.UserRoles
                .Where(link => link.UserId == actor.Id && !link.Role.IsDeleted)
                .SelectMany(link => link.Role.RolePermissions)
                .AnyAsync(link => !link.Permission.IsDeleted && link.Permission.Name == permission, token);
            if (!allowed)
                throw new OperationalAccessException(StatusCodes.Status403Forbidden, "El usuario no dispone del permiso requerido para esta operación");
            await failures.CheckpointAsync(ApplicationFailureCheckpoints.OperationalAfterAdmission, token);
            return transaction;
        }
        catch
        {
            await transaction.DisposeAsync();
            throw;
        }
    }
}
