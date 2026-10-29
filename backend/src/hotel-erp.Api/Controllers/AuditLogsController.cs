using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Database;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers
{
    public record AuditLogDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserFullName { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? EntityName { get; set; }
        public Guid? EntityId { get; set; }
        public string? CorrelativeNumber { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PreviousHash { get; set; }
        public string? Hash { get; set; }
        public string? Changes { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime HondurasTimestamp { get; set; }
    }

    [ApiController]
    [Route("api/audit-logs")]
    [Authorize(Policy = PermissionNames.ViewAudit)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AuditLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditChainService _auditChain;
        private readonly IHotelClock _clock;
        private readonly HotelListQueryService _lists;

        public AuditLogsController(ApplicationDbContext context, AuditChainService auditChain, IHotelClock clock, HotelListQueryService lists)
        {
            _context = context;
            _auditChain = auditChain;
            _clock = clock;
            _lists = lists;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetLogs(
            [FromQuery] string? action,
            [FromQuery] string? entityName,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? search)
        {
            var query = _context.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(action))
                query = query.Where(a => a.Action.ToLower().Contains(action.ToLower()));

            if (!string.IsNullOrWhiteSpace(entityName))
                query = query.Where(a => a.EntityName != null && a.EntityName.ToLower().Contains(entityName.ToLower()));

            if (startDate.HasValue)
                query = query.Where(a => a.Timestamp >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Timestamp <= endDate.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(a =>
                    a.Action.ToLower().Contains(s) ||
                    (a.EntityName != null && a.EntityName.ToLower().Contains(s)) ||
                    (a.CorrelativeNumber != null && a.CorrelativeNumber.ToLower().Contains(s)) ||
                    (a.User != null && (a.User.Username.ToLower().Contains(s) || a.User.FirstName.ToLower().Contains(s) || a.User.LastName.ToLower().Contains(s))));
            }

            var logs = await query
                .OrderByDescending(a => a.Timestamp)
                .Take(200)
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    UserName = a.User != null ? a.User.Username : null,
                    UserFullName = a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : "Sistema",
                    Action = a.Action,
                    EntityName = a.EntityName,
                    EntityId = a.EntityId,
                    CorrelativeNumber = a.CorrelativeNumber,
                    PaymentMethod = a.PaymentMethod,
                    PreviousHash = a.PreviousHash,
                    Hash = a.Hash,
                    Changes = a.Changes,
                    Timestamp = a.Timestamp,
                    HondurasTimestamp = a.HondurasTimestamp
                })
                .ToListAsync();

            return Ok(logs);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<AuditLogDto>>> GetPage([FromQuery] AuditPageQuery query)
            => Ok(await _lists.AuditAsync(query, HttpContext.RequestAborted));

        [HttpGet("actions")]
        public async Task<ActionResult<IEnumerable<string>>> GetActions()
            => Ok(await _context.AuditLogs.AsNoTracking().Select(log => log.Action)
                .Distinct().OrderBy(action => action).ToListAsync(HttpContext.RequestAborted));

        [HttpGet("verify-integrity")]
        public async Task<ActionResult<AuditIntegrityResult>> VerifyIntegrity(CancellationToken cancellationToken)
        {
            return Ok(await _auditChain.VerifyAsync(cancellationToken));
        }

        [HttpGet("export-integrity")]
        [ProducesResponseType(typeof(AuditChainExport), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportIntegrity(CancellationToken cancellationToken)
        {
            var export = await _auditChain.ExportAsync(cancellationToken);
            var fileName = $"hotel-erp-audit-chain-{_clock.UtcNow:yyyyMMddTHHmmssZ}.json";
            return File(AuditChainService.SerializeExport(export), "application/json", fileName);
        }
    }
}
