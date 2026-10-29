using System.Security.Claims;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Payments;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/payments/{paymentId:guid}/refunds")]
    [Authorize(Policy = PermissionNames.ManageCash)]
    public class RefundsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RefundApplicationService _refundService;
        private readonly IMapper _mapper;

        public RefundsController(
            ApplicationDbContext context,
            RefundApplicationService refundService,
            IMapper mapper)
        {
            _context = context;
            _refundService = refundService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefundDto>>> GetAll(Guid paymentId)
        {
            var exists = await _context.Payments.AnyAsync(payment => payment.Id == paymentId);
            if (!exists) return NotFound();
            var refunds = await DetailedRefunds(paymentId)
                .OrderByDescending(refund => refund.RefundDate)
                .AsSplitQuery()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RefundDto>>(refunds));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RefundDto>> GetById(Guid paymentId, Guid id)
        {
            var refund = await DetailedRefunds(paymentId)
                .AsSplitQuery()
                .SingleOrDefaultAsync(candidate => candidate.Id == id);
            return refund is null ? NotFound() : Ok(_mapper.Map<RefundDto>(refund));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RefundDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<RefundDto>> Create(
            Guid paymentId,
            [FromBody] CreateRefundRequest request,
            [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _refundService.CreateAsync(
                userId,
                paymentId,
                request,
                idempotencyKey,
                HttpContext.RequestAborted);
            if (!result.Succeeded)
                return StatusCode(result.FailureStatus!.Value, result.FailureDetail);

            var refund = result.Refund!;
            return CreatedAtAction(
                nameof(GetById),
                new { paymentId, id = refund.Id },
                _mapper.Map<RefundDto>(refund));
        }

        private IQueryable<Refund> DetailedRefunds(Guid paymentId) => _context.Refunds
            .Where(refund => refund.PaymentId == paymentId)
            .Include(refund => refund.Payment)
            .Include(refund => refund.RecordedByUser)
            .Include(refund => refund.CashRegister)
            .Include(refund => refund.AccountingEntry)
            .Include(refund => refund.Applications)
            .ThenInclude(application => application.CreditNote);
    }
}
