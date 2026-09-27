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
    [Route("api/card-settlements")]
    [Authorize(Policy = PermissionNames.ManageAccounting)]
    public class CardSettlementsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CardSettlementApplicationService _settlementService;
        private readonly IMapper _mapper;

        public CardSettlementsController(
            ApplicationDbContext context,
            CardSettlementApplicationService settlementService,
            IMapper mapper)
        {
            _context = context;
            _settlementService = settlementService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardSettlementDto>>> GetAll()
        {
            var settlements = await DetailedSettlements()
                .OrderByDescending(settlement => settlement.SettlementDate)
                .ThenByDescending(settlement => settlement.CreatedAt)
                .AsSplitQuery()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CardSettlementDto>>(settlements));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CardSettlementDto>> GetById(Guid id)
        {
            var settlement = await DetailedSettlements()
                .AsSplitQuery()
                .SingleOrDefaultAsync(candidate => candidate.Id == id);
            return settlement is null
                ? NotFound()
                : Ok(_mapper.Map<CardSettlementDto>(settlement));
        }

        [HttpGet("eligible-payments")]
        public async Task<ActionResult<IEnumerable<EligibleCardPaymentDto>>> GetEligiblePayments()
        {
            var payments = await _context.Payments
                .Where(payment => payment.Method == PaymentMethod.Tarjeta
                    && payment.Status != PaymentStatus.Anulado)
                .OrderBy(payment => payment.PaymentDate)
                .Select(payment => new
                {
                    payment.Id,
                    payment.PaymentNumber,
                    payment.PaymentDate,
                    payment.ExternalReference,
                    payment.Amount,
                    RefundedAmount = payment.Refunds
                        .Where(refund => refund.Status == RefundStatus.Confirmado)
                        .Sum(refund => (decimal?)refund.Amount) ?? 0m,
                    SettledAmount = payment.CardSettlementApplications
                        .Where(application => application.CardSettlement.Status == CardSettlementStatus.Confirmado)
                        .Sum(application => (decimal?)application.Amount) ?? 0m
                })
                .ToListAsync();

            var eligible = payments
                .Select(payment =>
                {
                    return new EligibleCardPaymentDto
                    {
                        Id = payment.Id,
                        PaymentNumber = payment.PaymentNumber,
                        PaymentDate = payment.PaymentDate,
                        ExternalReference = payment.ExternalReference,
                        Amount = payment.Amount,
                        RefundedAmount = payment.RefundedAmount,
                        SettledAmount = payment.SettledAmount,
                        AvailableAmount = Math.Max(
                            0m,
                            TaxService.RoundCurrency(
                                payment.Amount - payment.RefundedAmount - payment.SettledAmount))
                    };
                })
                .Where(payment => payment.AvailableAmount > 0m)
                .ToList();
            return Ok(eligible);
        }

        [HttpPost]
        public async Task<ActionResult<CardSettlementDto>> Create(
            [FromBody] CreateCardSettlementRequest request,
            [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _settlementService.CreateAsync(
                userId,
                request,
                idempotencyKey,
                HttpContext.RequestAborted);
            if (!result.Succeeded)
                return StatusCode(result.FailureStatus!.Value, result.FailureDetail);

            var settlement = result.Settlement!;
            return CreatedAtAction(
                nameof(GetById),
                new { id = settlement.Id },
                _mapper.Map<CardSettlementDto>(settlement));
        }

        private IQueryable<CardSettlement> DetailedSettlements() => _context.CardSettlements
            .Include(settlement => settlement.RecordedByUser)
            .Include(settlement => settlement.AccountingEntry)
            .Include(settlement => settlement.Applications)
            .ThenInclude(application => application.Payment);
    }
}
