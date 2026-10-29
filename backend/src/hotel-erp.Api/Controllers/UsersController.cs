using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionNames.ManageUsers)]
public class UsersController(IUserRepository repository, IMapper mapper, HotelListQueryService lists,
    SecurityAdministrationService security) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        => Ok(mapper.Map<IEnumerable<UserDto>>(await repository.GetAllAsync()));

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetPage([FromQuery] UserPageQuery query)
        => Ok(await lists.UsersAsync(query, HttpContext.RequestAborted));

    [HttpGet("role-options/paged")]
    public async Task<ActionResult<PagedResult<RoleOptionDto>>> GetRoleOptions([FromQuery] RoleOptionPageQuery query)
        => Ok(await lists.RoleOptionsAsync(query, User.FindAll(SecurityClaimTypes.Permission)
            .Select(claim => claim.Value).ToArray(), HttpContext.RequestAborted));

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        return user is null ? NotFound("Usuario no encontrado") : Ok(mapper.Map<UserDto>(user));
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest request)
    {
        var result = await security.CreateUserAsync(SecurityActor.From(User), request, HttpContext.RequestAborted);
        return result.Succeeded ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value) : Failure(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var result = await security.UpdateUserAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpPost("{id}/reset-password")]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> ResetPassword(Guid id, [FromBody] AdminResetPasswordRequest request)
    {
        var result = await security.ResetUserPasswordAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await security.DeactivateUserAsync(SecurityActor.From(User), id, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpPost("{userId}/roles/{roleId}")]
    [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> AssignRole(Guid userId, Guid roleId)
    {
        var result = await security.AssignRoleAsync(SecurityActor.From(User), userId, roleId, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    private ObjectResult Failure<T>(ApplicationMutationResult<T> result) where T : class
        => StatusCode(result.FailureStatus!.Value, result.FailureDetail);
}
