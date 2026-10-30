using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers;

[ApiController]
[Route("api/purchase-invoices")]
[Authorize(Policy = PermissionNames.ManageAccounting)]
public class PurchaseInvoicesController(
    IPurchaseInvoiceRepository repository,
    PurchaseInvoiceApplicationService purchaseService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseInvoiceDto>>> GetAll([FromQuery] Guid? supplierId)
    {
        var purchases = supplierId.HasValue
            ? await repository.GetBySupplierAsync(supplierId.Value)
            : await repository.GetAllAsync();
        return Ok(mapper.Map<IEnumerable<PurchaseInvoiceDto>>(purchases));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseInvoiceDto>> GetById(Guid id)
    {
        var purchase = await repository.GetByIdAsync(id);
        return purchase is null ? NotFound() : Ok(mapper.Map<PurchaseInvoiceDto>(purchase));
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<PurchaseInvoiceDto>>> GetByDateRange(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end)
        => Ok(mapper.Map<IEnumerable<PurchaseInvoiceDto>>(
            await repository.GetByDateRangeAsync(start, end)));

    [HttpPost]
    [ProducesResponseType(typeof(PurchaseInvoiceDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PurchaseInvoiceDto>> Create([FromBody] CreatePurchaseInvoiceRequest request)
    {
        var result = await purchaseService.CreateAsync(CurrentActor(), request, HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        var dto = mapper.Map<PurchaseInvoiceDto>(result.Value);
        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, dto);
    }

    [HttpPut("{id}/pay")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> MarkAsPaid(Guid id)
    {
        var result = await purchaseService.MarkAsPaidAsync(id, CurrentActor(), HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await purchaseService.DeleteAsync(id, CurrentActor(), HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    private SecurityActor CurrentActor()
        => SecurityActor.From(User);

    private ObjectResult Failure<T>(ApplicationMutationResult<T> result)
        where T : class
        => StatusCode(result.FailureStatus!.Value, result.FailureDetail);
}
