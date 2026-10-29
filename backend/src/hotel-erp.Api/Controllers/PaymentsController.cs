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
    [Route("api/payments")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PaymentApplicationService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(
            ApplicationDbContext context,
            PaymentApplicationService paymentService,
            IMapper mapper)
        {
            _context = context;
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = PermissionNames.ManageCash)]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAll([FromQuery] Guid? invoiceId)
        {
            var query = DetailedPayments().OrderByDescending(payment => payment.PaymentDate).AsQueryable();
            if (invoiceId.HasValue)
                query = query.Where(payment => payment.Applications.Any(application => application.InvoiceId == invoiceId.Value));
            return Ok(_mapper.Map<IEnumerable<PaymentDto>>(await query.AsSplitQuery().ToListAsync()));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionNames.ManageCash)]
        public async Task<ActionResult<PaymentDto>> GetById(Guid id)
        {
            var payment = await DetailedPayments().AsSplitQuery().SingleOrDefaultAsync(candidate => candidate.Id == id);
            return payment is null ? NotFound() : Ok(_mapper.Map<PaymentDto>(payment));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageCash)]
        [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<PaymentDto>> Create(
            [FromBody] CreatePaymentRequest request,
            [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _paymentService.CreateAsync(
                userId,
                request,
                idempotencyKey,
                HttpContext.RequestAborted);
            if (!result.Succeeded)
                return StatusCode(result.FailureStatus!.Value, result.FailureDetail);

            var payment = result.Payment!;
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, _mapper.Map<PaymentDto>(payment));
        }

        private IQueryable<Payment> DetailedPayments() => _context.Payments
            .Include(payment => payment.RecordedByUser)
            .Include(payment => payment.CashRegister)
            .Include(payment => payment.AccountingEntry)
            .Include(payment => payment.Applications)
            .ThenInclude(application => application.Invoice)
            .Include(payment => payment.Refunds)
            .ThenInclude(refund => refund.RecordedByUser)
            .Include(payment => payment.Refunds)
            .ThenInclude(refund => refund.CashRegister)
            .Include(payment => payment.Refunds)
            .ThenInclude(refund => refund.Applications)
            .ThenInclude(application => application.CreditNote)
            .Include(payment => payment.CardSettlementApplications)
            .ThenInclude(application => application.CardSettlement);
    }
}
