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
[Authorize(Policy = HotelAccessPolicies.ReadInvoices)]
public class InvoicesController(
    IInvoiceRepository repository,
    InvoiceApplicationService invoiceService,
    HotelListQueryService lists,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll()
        => Ok(mapper.Map<IEnumerable<InvoiceDto>>(await repository.GetAllAsync()));

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<InvoiceDto>>> GetPage([FromQuery] InvoicePageQuery query)
        => Ok(await lists.InvoicesAsync(query, HttpContext.RequestAborted));

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetById(Guid id)
    {
        var invoice = await repository.GetByIdAsync(id);
        if (invoice is null)
            return NotFound();

        return Ok(mapper.Map<InvoiceDto>(invoice));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> Search(
        [FromQuery] string? dni,
        [FromQuery] DateTime? start,
        [FromQuery] DateTime? end,
        [FromQuery] Guid? caiId,
        [FromQuery] Guid? documentAuthorizationId,
        [FromQuery] Guid? originalInvoiceId)
    {
        if (originalInvoiceId.HasValue)
            return Ok(mapper.Map<IEnumerable<InvoiceDto>>(await repository.GetByOriginalInvoiceAsync(originalInvoiceId.Value)));
        if (caiId.HasValue || documentAuthorizationId.HasValue)
            return Ok(mapper.Map<IEnumerable<InvoiceDto>>(await repository.GetByAuthorizationAsync(caiId, documentAuthorizationId)));
        if (!string.IsNullOrEmpty(dni))
            return Ok(mapper.Map<IEnumerable<InvoiceDto>>(await repository.GetByGuestDocumentAsync(dni)));
        if (start.HasValue && end.HasValue)
            return Ok(mapper.Map<IEnumerable<InvoiceDto>>(await repository.GetByDateRangeAsync(start.Value, end.Value)));

        return Ok(mapper.Map<IEnumerable<InvoiceDto>>(await repository.GetAllAsync()));
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetByDateRange(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end)
        => Ok(mapper.Map<IEnumerable<InvoiceDto>>(await repository.GetByDateRangeAsync(start, end)));

    [HttpPut("{id}")]
    [Authorize(Policy = PermissionNames.CreateInvoices)]
    public async Task<ActionResult<InvoiceDto>> Update(Guid id, [FromBody] CreateInvoiceRequest request)
    {
        var result = await invoiceService.UpdateDraftAsync(
            id,
            CurrentUserId(),
            request,
            HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        return Ok(mapper.Map<InvoiceDto>(result.Invoice));
    }

    [HttpPost]
    [Authorize(Policy = PermissionNames.CreateInvoices)]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<InvoiceDto>> Create(
        [FromBody] CreateInvoiceRequest request,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        var result = await invoiceService.CreateAsync(
            CurrentUserId(),
            request,
            idempotencyKey,
            HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        SetIdempotencyHeader(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Invoice!.Id }, mapper.Map<InvoiceDto>(result.Invoice));
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Policy = PermissionNames.CreateInvoices)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Cancel(Guid id)
    {
        var invoice = await repository.GetByIdAsync(id);
        if (invoice is null)
            return NotFound();

        return BadRequest("La anulación fiscal debe realizarse mediante nota de crédito vinculada");
    }

    [HttpPost("{id}/credit-note")]
    [Authorize(Policy = PermissionNames.AdjustInvoices)]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<InvoiceDto>> CreateCreditNote(
        Guid id,
        [FromBody] CreateCreditNoteRequest request,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        var result = await invoiceService.CreateCreditNoteAsync(
            id,
            CurrentUserId(),
            request,
            idempotencyKey,
            HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        SetIdempotencyHeader(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Invoice!.Id }, mapper.Map<InvoiceDto>(result.Invoice));
    }

    [HttpPost("{id}/debit-note")]
    [Authorize(Policy = PermissionNames.AdjustInvoices)]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<InvoiceDto>> CreateDebitNote(
        Guid id,
        [FromBody] CreateDebitNoteRequest request,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        var result = await invoiceService.CreateDebitNoteAsync(
            id,
            CurrentUserId(),
            request,
            idempotencyKey,
            HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        SetIdempotencyHeader(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Invoice!.Id }, mapper.Map<InvoiceDto>(result.Invoice));
    }

    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private ObjectResult Failure(InvoiceMutationResult result)
    {
        if (result.FailureTitle is not null)
        {
            return StatusCode(result.FailureStatus!.Value, new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{result.FailureStatus.Value}",
                Title = result.FailureTitle,
                Status = result.FailureStatus.Value,
                Detail = result.FailureDetail
            });
        }

        return StatusCode(result.FailureStatus!.Value, result.FailureDetail);
    }

    private void SetIdempotencyHeader(InvoiceMutationResult result)
    {
        if (result.IdempotencyKey is not null)
            Response.Headers["Idempotency-Key"] = result.IdempotencyKey;
    }
}
