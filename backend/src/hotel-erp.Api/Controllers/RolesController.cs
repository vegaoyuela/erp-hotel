using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionNames.ManageRoles)]
public class RolesController(IRoleRepository repository, IPermissionRepository permissions, IMapper mapper,
    SecurityAdministrationService security) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
        => Ok(mapper.Map<IEnumerable<RoleDto>>(await repository.GetAllAsync()));

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(Guid id)
    {
        var role = await repository.GetByIdAsync(id);
        return role is null ? NotFound() : Ok(mapper.Map<RoleDto>(role));
    }

    [HttpPost]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleRequest request)
    {
        var result = await security.CreateRoleAsync(SecurityActor.From(User), request, HttpContext.RequestAborted);
        return result.Succeeded ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value) : Failure(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateRoleRequest request)
    {
        var result = await security.UpdateRoleAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await security.DeleteRoleAsync(SecurityActor.From(User), id, HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    [HttpGet("permissions")]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
        => Ok(mapper.Map<IEnumerable<PermissionDto>>(await permissions.GetAllAsync()));

    private ObjectResult Failure<T>(ApplicationMutationResult<T> result) where T : class
        => StatusCode(result.FailureStatus!.Value, result.FailureDetail);
}
