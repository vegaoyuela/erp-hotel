using System.Text.Json;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using hotel_erp.Api.Database;

namespace hotel_erp.Api.Services
{
    public class AuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ApplicationDbContext _context;
        private readonly IHotelClock _clock;

        public AuditService(IAuditLogRepository auditLogRepository, ApplicationDbContext context, IHotelClock clock)
        {
            _auditLogRepository = auditLogRepository;
            _context = context;
            _clock = clock;
        }

        public async Task LogAsync(Guid? userId, string action, string? entityName, Guid? entityId, object? changes = null, string? correlativeNumber = null, string? paymentMethod = null)
        {
            await PostgresCorrelativeLock.ExecuteAsync(_context, _clock, "audit-chain", async () =>
            {
                var previousHash = await _context.AuditLogs
                    .Where(candidate => candidate.Hash != null
                        && !_context.AuditLogs.Any(next => next.PreviousHash == candidate.Hash))
                    .Select(candidate => candidate.Hash)
                    .SingleOrDefaultAsync();
                var serializedChanges = changes != null ? JsonSerializer.Serialize(changes) : null;
                var utcNow = _clock.UtcNow;
                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = action,
                    EntityName = entityName,
                    EntityId = entityId,
                    CorrelativeNumber = correlativeNumber,
                    PaymentMethod = paymentMethod,
                    Changes = serializedChanges,
                    Timestamp = utcNow,
                    HondurasTimestamp = _clock.HondurasNow,
                    PreviousHash = previousHash
                };
                auditLog.Hash = AuditChainService.ComputeHash(auditLog);

                await _auditLogRepository.AddAsync(auditLog);
                return true;
            });
        }

    }
}
