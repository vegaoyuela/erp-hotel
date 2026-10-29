using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed partial class SecurityAdministrationService
{
    public Task<ApplicationMutationResult<UserDto>> CreateUserAsync(SecurityActor actor, CreateUserRequest request,
        CancellationToken token, bool receptionRegistration = false)
        => ExecuteAsync(actor, PermissionNames.ManageUsers, receptionRegistration ? "RegisterReception" : "CreateUser", nameof(User), null, async state =>
        {
            var username = request.Username.Trim();
            var email = request.Email.Trim().ToLowerInvariant();
            if (await context.Users.AnyAsync(row => row.Username.ToLower() == username.ToLower() || row.Email.ToLower() == email, token))
                return ApplicationMutationResult<UserDto>.BadRequest("El nombre de usuario o correo electrónico ya está registrado");
            var roles = await ResolveRolesAsync(request.Roles, true, token);
            if (roles is null) return ApplicationMutationResult<UserDto>.BadRequest("Uno o más roles solicitados no existen");
            if (!await CanAssignRolesAsync(state, roles, token)) return Denied<UserDto>();
            var user = new User
            {
                Username = username,
                Email = email,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true,
                MustChangePassword = true
            };
            context.Users.Add(user);
            foreach (var role in roles) context.UserRoles.Add(new UserRole { User = user, Role = role });
            var value = await PersistAsync(actor, receptionRegistration ? "Register" : "CreateUser", nameof(User), user.Id,
                new { Roles = roles.Select(role => role.Id).ToArray(), user.IsActive, user.MustChangePassword }, [],
                () => UserDtoAsync(user.Id, token), token);
            if (receptionRegistration)
                value.Permissions = (await UserPermissionsAsync(user.Id, token)).Order().ToList();
            return ApplicationMutationResult<UserDto>.Success(value);
        }, token);

    public Task<ApplicationMutationResult<UserDto>> UpdateUserAsync(SecurityActor actor, Guid id, UpdateUserRequest request, CancellationToken token)
        => ExecuteAsync(actor, PermissionNames.ManageUsers, "UpdateUser", nameof(User), id, async state =>
        {
            var user = await context.Users.Include(row => row.UserRoles).ThenInclude(link => link.Role).SingleOrDefaultAsync(row => row.Id == id && !row.IsDeleted, token);
            if (user is null) return ApplicationMutationResult<UserDto>.NotFound("Usuario no encontrado");
            if (!await CanManageUserAsync(state, id, token)) return Denied<UserDto>();
            var roles = request.Roles is null ? null : await ResolveRolesAsync(request.Roles, false, token);
            if (request.Roles is not null && roles is null) return ApplicationMutationResult<UserDto>.BadRequest("Uno o más roles solicitados no existen");
            if (roles is not null && !await CanAssignRolesAsync(state, roles, token)) return Denied<UserDto>();
            if (request.IsActive == false && id == actor.Id) return ApplicationMutationResult<UserDto>.BadRequest("No puede desactivar su propia cuenta");
            var hadAdministrator = user.UserRoles.Any(link => !link.Role.IsDeleted && link.Role.SystemKey == SystemRoleKeys.Administrator);
            var keepsAdministrator = roles is null ? hadAdministrator : roles.Any(role => role.SystemKey == SystemRoleKeys.Administrator);
            if (user.IsActive && hadAdministrator && (!(request.IsActive ?? user.IsActive) || !keepsAdministrator)
                && !await HasOtherAdministratorAsync(id, token))
                return ApplicationMutationResult<UserDto>.Conflict("Debe conservar al menos un administrador activo");
            var email = request.Email?.Trim().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(email) && await context.Users.AnyAsync(row => row.Id != id && row.Email.ToLower() == email, token))
                return ApplicationMutationResult<UserDto>.BadRequest("El correo electrónico ya pertenece a otro usuario");
            var firstNameChanged = request.FirstName is not null && user.FirstName != request.FirstName.Trim();
            var lastNameChanged = request.LastName is not null && user.LastName != request.LastName.Trim();
            var emailChanged = email is not null && user.Email != email;
            var previousActive = user.IsActive;
            var previousRoles = user.UserRoles.Select(link => link.RoleId).Order().ToArray();
            // All rejection paths precede preparing tracked changes or writing audit.
            if (!string.IsNullOrWhiteSpace(request.FirstName)) user.FirstName = request.FirstName.Trim();
            if (!string.IsNullOrWhiteSpace(request.LastName)) user.LastName = request.LastName.Trim();
            if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
            if (request.IsActive.HasValue) user.IsActive = request.IsActive.Value;
            if (roles is not null)
            {
                var desired = roles.Select(role => role.Id).ToHashSet();
                var existing = user.UserRoles.Select(link => link.RoleId).ToHashSet();
                context.UserRoles.RemoveRange(user.UserRoles.Where(link => !desired.Contains(link.RoleId)));
                foreach (var role in roles.Where(role => !existing.Contains(role.Id)))
                    context.UserRoles.Add(new UserRole { UserId = id, RoleId = role.Id });
            }
            var dto = await PersistAsync(actor, "UpdateUser", nameof(User), id,
                new
                {
                    FirstNameChanged = firstNameChanged,
                    LastNameChanged = lastNameChanged,
                    EmailChanged = emailChanged,
                    PreviousActive = previousActive,
                    user.IsActive,
                    PreviousRoles = previousRoles,
                    RoleIds = roles?.Select(role => role.Id).Order().ToArray()
                },
                [id], () => UserDtoAsync(id, token), token);
            return ApplicationMutationResult<UserDto>.Success(dto);
        }, token);

    public Task<ApplicationMutationResult<MessageResponseDto>> ResetUserPasswordAsync(SecurityActor actor, Guid id, AdminResetPasswordRequest request, CancellationToken token)
        => ExecuteAsync(actor, PermissionNames.ManageUsers, "ResetUserPassword", nameof(User), id, async state =>
        {
            var user = await context.Users.SingleOrDefaultAsync(row => row.Id == id && !row.IsDeleted, token);
            if (user is null) return ApplicationMutationResult<MessageResponseDto>.NotFound("Usuario no encontrado");
            if (!await CanManageUserAsync(state, id, token)) return Denied<MessageResponseDto>();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.MustChangePassword = true;
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            var dto = await PersistAsync(actor, "ResetUserPassword", nameof(User), id, new { MustChangePassword = true },
                [id], () => Task.FromResult(new MessageResponseDto("Contraseña restablecida exitosamente")), token);
            return ApplicationMutationResult<MessageResponseDto>.Success(dto);
        }, token);

    public Task<ApplicationMutationResult<MessageResponseDto>> DeactivateUserAsync(SecurityActor actor, Guid id, CancellationToken token)
        => ExecuteAsync(actor, PermissionNames.ManageUsers, "DeactivateUser", nameof(User), id, async state =>
        {
            var user = await context.Users.Include(row => row.UserRoles).ThenInclude(link => link.Role).SingleOrDefaultAsync(row => row.Id == id && !row.IsDeleted, token);
            if (user is null) return ApplicationMutationResult<MessageResponseDto>.NotFound("Usuario no encontrado");
            if (!await CanManageUserAsync(state, id, token)) return Denied<MessageResponseDto>();
            if (id == actor.Id) return ApplicationMutationResult<MessageResponseDto>.BadRequest("No puede desactivar su propia cuenta");
            if (user.IsActive && user.UserRoles.Any(link => !link.Role.IsDeleted && link.Role.SystemKey == SystemRoleKeys.Administrator)
                && !await HasOtherAdministratorAsync(id, token))
                return ApplicationMutationResult<MessageResponseDto>.Conflict("Debe conservar al menos un administrador activo");
            user.IsActive = false;
            var dto = await PersistAsync(actor, "DeactivateUser", nameof(User), id, new { IsActive = false }, [id],
                () => Task.FromResult(new MessageResponseDto("Usuario desactivado exitosamente")), token);
            return ApplicationMutationResult<MessageResponseDto>.Success(dto);
        }, token);

    public Task<ApplicationMutationResult<MessageResponseDto>> AssignRoleAsync(SecurityActor actor, Guid id, Guid roleId, CancellationToken token)
        => ExecuteAsync(actor, PermissionNames.ManageUsers, "AssignRole", nameof(User), id, async state =>
        {
            var user = await context.Users.SingleOrDefaultAsync(row => row.Id == id && !row.IsDeleted, token);
            if (user is null) return ApplicationMutationResult<MessageResponseDto>.NotFound("Usuario no encontrado");
            if (!await CanManageUserAsync(state, id, token)) return Denied<MessageResponseDto>();
            var role = await context.Roles.SingleOrDefaultAsync(row => row.Id == roleId && !row.IsDeleted, token);
            if (role is null) return ApplicationMutationResult<MessageResponseDto>.NotFound("Rol no encontrado");
            if (!await CanAssignRolesAsync(state, [role], token)) return Denied<MessageResponseDto>();
            if (await context.UserRoles.AnyAsync(link => link.UserId == id && link.RoleId == roleId, token))
                return ApplicationMutationResult<MessageResponseDto>.BadRequest("El usuario ya tiene este rol");
            context.UserRoles.Add(new UserRole { UserId = id, RoleId = roleId });
            var dto = await PersistAsync(actor, "AssignRole", nameof(User), id, new { RoleId = roleId }, [id],
                () => Task.FromResult(new MessageResponseDto("Rol asignado exitosamente")), token);
            return ApplicationMutationResult<MessageResponseDto>.Success(dto);
        }, token);

    private Task<bool> HasOtherAdministratorAsync(Guid id, CancellationToken token)
        => context.Users.AnyAsync(row => row.Id != id && row.IsActive && !row.IsDeleted && row.UserRoles.Any(link =>
            !link.Role.IsDeleted && link.Role.SystemKey == SystemRoleKeys.Administrator), token);

    private async Task<List<Role>?> ResolveRolesAsync(IEnumerable<string>? names, bool defaultReception, CancellationToken token)
    {
        var requested = (names ?? []).Select(SecurityCatalogSeeder.NormalizeRoleName).Distinct().ToArray();
        if (requested.Length == 0 && defaultReception)
        {
            var role = await context.Roles.SingleOrDefaultAsync(row => !row.IsDeleted && row.SystemKey == SystemRoleKeys.Reception, token);
            return role is null ? null : [role];
        }
        var roles = await context.Roles.Where(row => !row.IsDeleted && requested.Contains(row.NormalizedName)).ToListAsync(token);
        return roles.Count == requested.Length ? roles : null;
    }
}
