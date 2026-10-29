using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed partial class HotelListQueryService
{
    public async Task<PagedResult<UserDto>> UsersAsync(UserPageQuery request, CancellationToken token)
    {
        var query = context.Users.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(row => row.Username.ToLower().Contains(term) || row.Email.ToLower().Contains(term)
                || (row.FirstName + " " + row.LastName).ToLower().Contains(term));
        }
        if (request.RoleId.HasValue) query = query.Where(row => row.UserRoles.Any(link => link.RoleId == request.RoleId));
        return await query.Select(row => new UserDto
        {
            Id = row.Id,
            Username = row.Username,
            Email = row.Email,
            FirstName = row.FirstName,
            LastName = row.LastName,
            IsActive = row.IsActive,
            MustChangePassword = row.MustChangePassword,
            Roles = row.UserRoles.Where(link => !link.Role.IsDeleted).OrderBy(link => link.Role.Name).Select(link => link.Role.Name).ToList()
        }).OrderBy(row => row.Username).ThenBy(row => row.Id).ToPageAsync(request, token);
    }

    public Task<PagedResult<RoleOptionDto>> RoleOptionsAsync(RoleOptionPageQuery request, string[] actorPermissions, CancellationToken token)
    {
        var query = context.Roles.AsNoTracking().Where(row => !row.IsDeleted);
        if (request.AssignableOnly)
            query = query.Where(row => !row.RolePermissions.Any(link => !link.Permission.IsDeleted && !actorPermissions.Contains(link.Permission.Name)));
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(row => row.Name.ToLower().Contains(term));
        }
        return query.Select(row => new RoleOptionDto { Id = row.Id, Name = row.Name }).OrderBy(row => row.Name).ThenBy(row => row.Id).ToPageAsync(request, token);
    }
}
