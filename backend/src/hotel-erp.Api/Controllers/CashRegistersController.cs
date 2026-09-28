using System.Security.Claims;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Dtos.Cash;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers;

[ApiController]
[Route("api/cash-registers")]
[Authorize]
public class CashRegistersController(
    ApplicationDbContext context,
    CashRegisterApplicationService cashRegisterService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = PermissionNames.ManageCash)]
    public async Task<ActionResult<IEnumerable<CashRegisterDto>>> GetAll()
    {
        var registers = await context.CashRegisters
            .AsNoTracking()
            .Include(register => register.CashMovements
                .OrderByDescending(movement => movement.CreatedAt)
                .ThenByDescending(movement => movement.Id)
                .Take(1))
            .OrderBy(register => register.Name)
            .ToListAsync(HttpContext.RequestAborted);

        return Ok(registers.Select(register =>
            CashRegisterApplicationService.ToDto(register, register.CashMovements.SingleOrDefault())));
    }

    [HttpPost]
    [Authorize(Policy = PermissionNames.ManageCash)]
    public async Task<ActionResult<CashRegisterDto>> Create([FromBody] CreateCashRegisterRequest request)
    {
        var result = await cashRegisterService.CreateAsync(CurrentUserId(), request, HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        return CreatedAtAction(nameof(GetAll), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPost("{id}/open")]
    [Authorize(Policy = PermissionNames.ManageCash)]
    public async Task<ActionResult> Open(
        Guid id,
        [FromBody] OpenCashRegisterRequest request,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        var result = await cashRegisterService.OpenAsync(
            id,
            CurrentUserId(),
            request,
            idempotencyKey,
            HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpPost("{id}/close")]
    [Authorize(Policy = PermissionNames.ManageCash)]
    public async Task<ActionResult> Close(
        Guid id,
        [FromBody] CloseCashRegisterRequest request,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        var result = await cashRegisterService.CloseAsync(
            id,
            CurrentUserId(),
            request,
            idempotencyKey,
            HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpGet("{id}/movements")]
    [Authorize(Policy = PermissionNames.ManageCash)]
    public async Task<ActionResult<IEnumerable<CashMovementDto>>> GetMovements(Guid id)
    {
        if (!await context.CashRegisters.AnyAsync(register => register.Id == id, HttpContext.RequestAborted))
            return NotFound();

        var movements = await context.CashMovements
            .AsNoTracking()
            .Include(movement => movement.CashRegister)
            .Include(movement => movement.User)
            .Where(movement => movement.CashRegisterId == id)
            .OrderByDescending(movement => movement.CreatedAt)
            .ThenByDescending(movement => movement.Id)
            .ToListAsync(HttpContext.RequestAborted);
        return Ok(mapper.Map<IEnumerable<CashMovementDto>>(movements));
    }

    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private ObjectResult Failure<T>(ApplicationMutationResult<T> result)
        where T : class
        => StatusCode(result.FailureStatus!.Value, result.FailureDetail);
}
