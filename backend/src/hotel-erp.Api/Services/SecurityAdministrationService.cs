using System.Security.Claims;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record SecurityActor(Guid Id, int Version)
{
    public static SecurityActor From(ClaimsPrincipal principal)
        => new(Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!),
            int.Parse(principal.FindFirstValue(SecurityClaimTypes.SecurityVersion)!, System.Globalization.CultureInfo.InvariantCulture));
}

public sealed partial class SecurityAdministrationService(
    ApplicationDbContext context, IMapper mapper, IAuditService audit, IHotelClock clock,
    IApplicationFailureInjector failureInjector)
{
    private sealed record ActorState(SecurityActor Actor, HashSet<string> Permissions);

    private async Task<ApplicationMutationResult<T>> ExecuteAsync<T>(SecurityActor actor, string permission,
        string operation, string entity, Guid? target, Func<ActorState, Task<ApplicationMutationResult<T>>> mutate,
        CancellationToken token) where T : class
    {
        await using var transaction = await SecurityMutationLocks.BeginAdministrationAsync(context, token);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterLock, token);
        // Authentication may have run before this request waited for the catalog lock.
        var current = await context.Users.AsNoTracking().SingleOrDefaultAsync(row => row.Id == actor.Id, token);
        if (current is null || !current.IsActive || current.IsDeleted || current.SecurityVersion != actor.Version)
            return new(null, StatusCodes.Status401Unauthorized, "La sesión fue revocada; vuelva a iniciar sesión");
        var permissions = await UserPermissionsAsync(actor.Id, token);
        if (current.MustChangePassword || !permissions.Contains(permission))
        {
            await audit.LogAsync(actor.Id, "SecurityChangeDenied", entity, target, new { Operation = operation });
            await transaction.CommitAsync(token);
            return Denied<T>();
        }
        var result = await mutate(new(actor, permissions));
        if (!result.Succeeded && result.FailureStatus != StatusCodes.Status404NotFound)
            await audit.LogAsync(actor.Id,
                result.FailureStatus == StatusCodes.Status403Forbidden ? "SecurityChangeDenied" : "SecurityChangeRejected",
                entity, target, new { Operation = operation, Status = result.FailureStatus });
        await transaction.CommitAsync(token);
        return result;
    }

    private async Task<HashSet<string>> UserPermissionsAsync(Guid userId, CancellationToken token)
        => (await context.UserRoles.Where(link => link.UserId == userId && !link.Role.IsDeleted)
            .SelectMany(link => link.Role.RolePermissions).Where(link => !link.Permission.IsDeleted)
            .Select(link => link.Permission.Name).Distinct().ToListAsync(token)).ToHashSet(StringComparer.Ordinal);

    private async Task<bool> CanManageUserAsync(ActorState actor, Guid target, CancellationToken token)
        => (await UserPermissionsAsync(target, token)).IsSubsetOf(actor.Permissions);

    private Task<bool> CanAssignRolesAsync(ActorState actor, IEnumerable<Role> roles, CancellationToken token)
    {
        var ids = roles.Select(role => role.Id).ToArray();
        return context.RolePermissions.Where(link => ids.Contains(link.RoleId) && !link.Permission.IsDeleted)
            .AllAsync(link => actor.Permissions.Contains(link.Permission.Name), token);
    }

    private async Task<T> PersistAsync<T>(SecurityActor actor, string operation, string entity, Guid target,
        object changes, IEnumerable<Guid> affectedUserIds, Func<Task<T>> response, CancellationToken token)
    {
        var ids = affectedUserIds.Distinct().ToArray();
        var affected = await context.Users.Where(row => ids.Contains(row.Id)).ToListAsync(token);
        foreach (var user in affected) user.SecurityVersion = checked(user.SecurityVersion + 1);
        await context.SaveChangesAsync(token);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterPersistence, token);
        var revokedAt = clock.UtcNow;
        await context.RefreshTokens.Where(row => ids.Contains(row.UserId) && !row.IsRevoked)
            .ExecuteUpdateAsync(setters => setters.SetProperty(row => row.IsRevoked, true)
                .SetProperty(row => row.RevokedAt, revokedAt), token);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterRevocation, token);
        await audit.LogAsync(actor.Id, operation, entity, target, changes);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.SecurityAfterAudit, token);
        return await response();
    }

    private async Task<UserDto> UserDtoAsync(Guid id, CancellationToken token)
        => mapper.Map<UserDto>(await context.Users.AsNoTracking().Include(row => row.UserRoles)
            .ThenInclude(link => link.Role).SingleAsync(row => row.Id == id, token));

    private static ApplicationMutationResult<T> Denied<T>() where T : class
        => new(null, StatusCodes.Status403Forbidden, "No puede conceder ni administrar permisos superiores a los propios");
}
