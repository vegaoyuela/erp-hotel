using System.Security.Claims;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoliosController(
    IFolioRepository repository,
    FolioApplicationService folioService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FolioDto>>> GetAll()
        => Ok(mapper.Map<IEnumerable<FolioDto>>(await repository.GetAllAsync()));

    [HttpGet("{id}")]
    public async Task<ActionResult<FolioDto>> GetById(Guid id)
    {
        var folio = await repository.GetByIdAsync(id);
        if (folio is null)
            return NotFound();

        return Ok(mapper.Map<FolioDto>(folio));
    }

    [HttpGet("by-reservation/{reservationId}")]
    public async Task<ActionResult<FolioDto>> GetByReservation(Guid reservationId)
    {
        var folio = await repository.GetByReservationAsync(reservationId);
        if (folio is null)
            return NotFound();

        return Ok(mapper.Map<FolioDto>(folio));
    }

    [HttpPost("{folioId}/items")]
    [Authorize(Policy = PermissionNames.CreateInvoices)]
    public async Task<ActionResult> AddItem(Guid folioId, [FromBody] AddFolioItemRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await folioService.AddItemAsync(folioId, userId, request, HttpContext.RequestAborted);
        if (!result.Succeeded)
            return StatusCode(result.FailureStatus!.Value, result.FailureDetail);

        return Ok(result.Value);
    }
}
