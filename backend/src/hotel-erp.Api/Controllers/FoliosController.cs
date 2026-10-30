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
[Authorize(Policy = HotelAccessPolicies.ReadFolios)]
public class FoliosController(
    IFolioRepository repository,
    FolioApplicationService folioService,
    IMapper mapper,
    HotelListQueryService lists) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FolioDto>>> GetAll()
        => Ok(mapper.Map<IEnumerable<FolioDto>>(await repository.GetAllAsync()));

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<FolioListDto>>> GetPage([FromQuery] FolioPageQuery query)
        => Ok(await lists.FoliosAsync(query, HttpContext.RequestAborted));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FolioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FolioDto>> GetById(Guid id)
    {
        var folio = await repository.GetByIdAsync(id);
        if (folio is null)
            return NotFound();

        return Ok(mapper.Map<FolioDto>(folio));
    }

    [HttpGet("by-reservation/{reservationId}")]
    [ProducesResponseType(typeof(FolioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FolioDto>> GetByReservation(Guid reservationId)
    {
        var folio = await repository.GetByReservationAsync(reservationId);
        if (folio is null)
            return NotFound();

        return Ok(mapper.Map<FolioDto>(folio));
    }

    [HttpPost("{folioId}/items")]
    [ProducesResponseType(typeof(FolioChargeResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = PermissionNames.CreateInvoices)]
    public async Task<ActionResult<FolioChargeResult>> AddItem(Guid folioId, [FromBody] AddFolioItemRequest request)
    {
        var actor = SecurityActor.From(User);
        var result = await folioService.AddItemAsync(folioId, actor, request, HttpContext.RequestAborted);
        if (!result.Succeeded)
            return StatusCode(result.FailureStatus!.Value, result.FailureDetail);

        return Ok(result.Value);
    }
}
