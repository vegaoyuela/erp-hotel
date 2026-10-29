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
public class ReservationsController(
    IReservationRepository repository,
    ReservationApplicationService reservationService,
    IHotelClock clock,
    IMapper mapper,
    HotelListQueryService lists) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAll([FromQuery] string? status)
    {
        if (!string.IsNullOrEmpty(status))
            return Ok(mapper.Map<IEnumerable<ReservationDto>>(await repository.GetByStatusAsync(status)));

        return Ok(mapper.Map<IEnumerable<ReservationDto>>(await repository.GetAllAsync()));
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<ReservationDto>>> GetPage([FromQuery] ReservationPageQuery query)
        => Ok(await lists.ReservationsAsync(query, HttpContext.RequestAborted));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> GetById(Guid id)
    {
        var reservation = await repository.GetByIdAsync(id);
        if (reservation is null)
            return NotFound();

        return Ok(mapper.Map<ReservationDto>(reservation));
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetByDateRange(
        [FromQuery] DateOnly start,
        [FromQuery] DateOnly end)
        => Ok(mapper.Map<IEnumerable<ReservationDto>>(await repository.GetByDateRangeAsync(start, end)));

    [HttpGet("calendar")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetCalendar(
        [FromQuery] DateOnly? start,
        [FromQuery] DateOnly? end)
    {
        var from = start ?? clock.Today.AddMonths(-1);
        var to = end ?? clock.Today.AddMonths(1);
        return Ok(mapper.Map<IEnumerable<ReservationDto>>(await repository.GetByDateRangeAsync(from, to)));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationRequest request)
    {
        var result = await reservationService.CreateAsync(CurrentUserId(), request, HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            mapper.Map<ReservationDto>(result.Value));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReservationVersionResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public async Task<ActionResult<ReservationVersionResult>> Update(Guid id, [FromBody] UpdateReservationRequest request)
    {
        var result = await reservationService.UpdateAsync(id, CurrentUserId(), request, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpPost("{id}/confirm")]
    [ProducesResponseType(typeof(ReservationStateResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public async Task<ActionResult<ReservationStateResult>> Confirm(Guid id, [FromBody] ConfirmReservationRequest request)
    {
        var result = await reservationService.ConfirmAsync(id, CurrentUserId(), request, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(ReservationStateResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public async Task<ActionResult<ReservationStateResult>> Cancel(Guid id, [FromBody] CancelReservationRequest request)
    {
        var result = await reservationService.CancelAsync(id, CurrentUserId(), request, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpPost("checkin")]
    [ProducesResponseType(typeof(CheckInResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public async Task<ActionResult<CheckInResult>> CheckIn([FromBody] CheckInRequest request)
    {
        var result = await reservationService.CheckInAsync(CurrentUserId(), request, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpPost("checkout/{id}")]
    [ProducesResponseType(typeof(CheckOutResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public async Task<ActionResult<CheckOutResult>> CheckOut(Guid id, [FromBody] CheckOutRequest request)
    {
        var result = await reservationService.CheckOutAsync(id, CurrentUserId(), request, HttpContext.RequestAborted);
        return result.Succeeded ? Ok(result.Value) : Failure(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (await repository.GetByIdAsync(id) is null)
            return NotFound();

        return BadRequest("Use la cancelación con motivo para preservar el historial de la reserva");
    }

    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private ObjectResult Failure<T>(ApplicationMutationResult<T> result)
        where T : class
        => StatusCode(result.FailureStatus!.Value, result.FailureDetail);
}
