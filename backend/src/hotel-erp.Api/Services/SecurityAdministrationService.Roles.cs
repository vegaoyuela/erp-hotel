using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed partial class SecurityAdministrationService
{
    public Task<ApplicationMutationResult<RoleDto>> CreateRoleAsync(SecurityActor actor, CreateRoleRequest request, CancellationToken token)
        => ExecuteAsync(actor, PermissionNames.ManageRoles, "CreateRole", nameof(Role), null, async state =>
        {
            var normalized = SecurityCatalogSeeder.NormalizeRoleName(request.Name);
            if (normalized.Length < 3) return ApplicationMutationResult<RoleDto>.BadRequest("El nombre del rol debe tener al menos 3 caracteres");
            if (await context.Roles.AnyAsync(row => row.NormalizedName == normalized, token))
                return ApplicationMutationResult<RoleDto>.BadRequest("El rol ya existe");
            var permissions = await ResolvePermissionsAsync(state, request.Permissions, token);
            if (permissions is null) return ApplicationMutationResult<RoleDto>.BadRequest("Uno o más permisos no existen o exceden los del operador");
            var role = new Role { Name = request.Name.Trim(), NormalizedName = normalized, Description = request.Description };
            context.Roles.Add(role);
            foreach (var permission in permissions) context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
            var dto = await PersistAsync(actor, "CreateRole", nameof(Role), role.Id,
                new { PermissionNames = permissions.Select(row => row.Name).Order().ToArray() }, [],
                () => RoleDtoAsync(role.Id, token), token);
            return ApplicationMutationResult<RoleDto>.Success(dto);
        }, token);

    public Task<ApplicationMutationResult<MessageResponseDto>> UpdateRoleAsync(SecurityActor actor, Guid id, UpdateRoleRequest request, CancellationToken token)
        => ExecuteAsync(actor, PermissionNames.ManageRoles, "UpdateRole", nameof(Role), id, async state =>
        {
            var role = await context.Roles.Include(row => row.RolePermissions).ThenInclude(link => link.Permission)
                .SingleOrDefaultAsync(row => row.Id == id && !row.IsDeleted, token);
            if (role is null) return ApplicationMutationResult<MessageResponseDto>.NotFound("Rol no encontrado");
            if (!await CanAssignRolesAsync(state, [role], token)) return Denied<MessageResponseDto>();
            var name = request.Name?.Trim();
            var normalized = name is null ? role.NormalizedName : SecurityCatalogSeeder.NormalizeRoleName(name);
            if (name is not null && name.Length < 3) return ApplicationMutationResult<MessageResponseDto>.BadRequest("El nombre del rol debe tener al menos 3 caracteres");
            if (role.SystemKey is not null && normalized != role.NormalizedName)
                return ApplicationMutationResult<MessageResponseDto>.BadRequest("Los roles del sistema no se pueden renombrar");
            if (await context.Roles.AnyAsync(row => row.Id != id && row.NormalizedName == normalized, token))
                return ApplicationMutationResult<MessageResponseDto>.BadRequest("El rol ya existe");
            var permissions = request.Permissions is null ? null : await ResolvePermissionsAsync(state, request.Permissions, token);
            if (request.Permissions is not null && permissions is null)
                return ApplicationMutationResult<MessageResponseDto>.BadRequest("Uno o más permisos no existen o exceden los del operador");
            if (role.SystemKey == SystemRoleKeys.Administrator && permissions is not null
                && PermissionNames.All.Except(permissions.Select(row => row.Name)).Any())
                return ApplicationMutationResult<MessageResponseDto>.Conflict("El rol administrador debe conservar los permisos del sistema");
            var previousName = role.Name;
            var previousPermissions = role.RolePermissions.Where(link => !link.Permission.IsDeleted).Select(link => link.Permission.Name).Order().ToArray();
            var affected = await context.UserRoles.Where(link => link.RoleId == id).Select(link => link.UserId).Distinct().ToArrayAsync(token);
            if (name is not null) { role.Name = name; role.NormalizedName = normalized; }
            if (request.Description is not null) role.Description = request.Description;
            if (permissions is not null)
            {
                var desired = permissions.Select(row => row.Id).ToHashSet();
                var existing = role.RolePermissions.Select(link => link.PermissionId).ToHashSet();
                context.RolePermissions.RemoveRange(role.RolePermissions.Where(link => !desired.Contains(link.PermissionId)));
                foreach (var permission in permissions.Where(row => !existing.Contains(row.Id)))
                    context.RolePermissions.Add(new RolePermission { RoleId = id, PermissionId = permission.Id });
            }
            var dto = await PersistAsync(actor, "UpdateRole", nameof(Role), id,
                new
                {
                    PreviousName = previousName,
                    role.Name,
                    DescriptionSupplied = request.Description is not null,
                    PreviousPermissions = previousPermissions,
                    PermissionNames = permissions?.Select(row => row.Name).Order().ToArray(),
                    AffectedUsers = affected.Length
                },
                affected, () => Task.FromResult(new MessageResponseDto("Rol actualizado")), token);
            return ApplicationMutationResult<MessageResponseDto>.Success(dto);
        }, token);

    public Task<ApplicationMutationResult<MessageResponseDto>> DeleteRoleAsync(SecurityActor actor, Guid id, CancellationToken token)
        => ExecuteAsync(actor, PermissionNames.ManageRoles, "DeleteRole", nameof(Role), id, async state =>
        {
            var role = await context.Roles.SingleOrDefaultAsync(row => row.Id == id && !row.IsDeleted, token);
            if (role is null) return ApplicationMutationResult<MessageResponseDto>.NotFound("Rol no encontrado");
            if (!await CanAssignRolesAsync(state, [role], token)) return Denied<MessageResponseDto>();
            if (role.SystemKey is not null) return ApplicationMutationResult<MessageResponseDto>.BadRequest("Los roles del sistema no se pueden eliminar");
            var affected = await context.UserRoles.Where(link => link.RoleId == id).Select(link => link.UserId).Distinct().ToArrayAsync(token);
            role.IsDeleted = true;
            var dto = await PersistAsync(actor, "DeleteRole", nameof(Role), id, new { AffectedUsers = affected.Length }, affected,
                () => Task.FromResult(new MessageResponseDto("Rol eliminado")), token);
            return ApplicationMutationResult<MessageResponseDto>.Success(dto);
        }, token);

    private async Task<List<Permission>?> ResolvePermissionsAsync(ActorState actor, IEnumerable<string>? names, CancellationToken token)
    {
        var requested = (names ?? []).Select(name => name.Trim()).Where(name => name.Length > 0).Distinct(StringComparer.Ordinal).ToArray();
        if (!requested.All(actor.Permissions.Contains)) return null;
        var permissions = await context.Permissions.Where(row => !row.IsDeleted && requested.Contains(row.Name)).ToListAsync(token);
        return permissions.Count == requested.Length ? permissions : null;
    }

    private async Task<RoleDto> RoleDtoAsync(Guid id, CancellationToken token)
        => mapper.Map<RoleDto>(await context.Roles.AsNoTracking().Include(row => row.RolePermissions).ThenInclude(link => link.Permission)
            .SingleAsync(row => row.Id == id, token));
}
