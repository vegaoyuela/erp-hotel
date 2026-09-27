using System.Globalization;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record AuditIntegrityResult(
    bool IsValid,
    int TotalRecordsVerified,
    DateTime VerifiedAt,
    string? ErrorCode = null,
    string? ErrorMessage = null,
    Guid? BrokenLogId = null);

public sealed record AuditChainExport(
    int SchemaVersion,
    DateTime GeneratedAt,
    AuditIntegrityResult Integrity,
    IReadOnlyList<AuditChainExportEntry> Records);

public sealed record AuditChainExportEntry(
    Guid Id,
    Guid? UserId,
    string Action,
    string? EntityName,
    Guid? EntityId,
    string? CorrelativeNumber,
    string? PaymentMethod,
    string? Changes,
    DateTime Timestamp,
    DateTime HondurasTimestamp,
    string? PreviousHash,
    string Hash);

public sealed class AuditChainService(ApplicationDbContext context, IHotelClock clock)
{
    public async Task<AuditIntegrityResult> VerifyAsync(CancellationToken cancellationToken = default)
    {
        var logs = await context.AuditLogs
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return Verify(logs, clock.UtcNow);
    }

    public async Task<AuditChainExport> ExportAsync(CancellationToken cancellationToken = default)
    {
        var logs = await context.AuditLogs
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var verifiedAt = clock.UtcNow;
        var integrity = Verify(logs, verifiedAt);
        var ordered = OrderChain(logs);
        var records = ordered.Select(log => new AuditChainExportEntry(
            log.Id,
            log.UserId,
            log.Action,
            log.EntityName,
            log.EntityId,
            log.CorrelativeNumber,
            log.PaymentMethod,
            log.Changes,
            NormalizeUtc(log.Timestamp),
            DateTime.SpecifyKind(log.HondurasTimestamp, DateTimeKind.Unspecified),
            NormalizeHash(log.PreviousHash),
            NormalizeHash(log.Hash) ?? string.Empty)).ToList();

        return new AuditChainExport(1, verifiedAt, integrity, records);
    }

    public static AuditIntegrityResult Verify(IReadOnlyCollection<AuditLog> logs, DateTime verifiedAt)
    {
        if (logs.Count == 0)
            return Success(0, verifiedAt);

        var byHash = new Dictionary<string, AuditLog>(StringComparer.OrdinalIgnoreCase);
        foreach (var log in logs)
        {
            var storedHash = NormalizeHash(log.Hash);
            if (storedHash is null)
                return Failure(0, verifiedAt, "AUDIT_HASH_MISSING", "Un registro no contiene hash.", log.Id);
            if (!byHash.TryAdd(storedHash, log))
                return Failure(0, verifiedAt, "AUDIT_HASH_DUPLICATE", "La cadena contiene hashes duplicados.", log.Id);

            if (!MatchesStoredHash(log, storedHash))
                return Failure(0, verifiedAt, "AUDIT_HASH_MISMATCH", "Un registro fue modificado o está corrupto.", log.Id);
        }

        var roots = logs.Where(log => NormalizeHash(log.PreviousHash) is null).ToList();
        if (roots.Count != 1)
            return Failure(0, verifiedAt, "AUDIT_ROOT_COUNT", "La cadena debe contener una única raíz.", roots.FirstOrDefault()?.Id);

        var childrenByPreviousHash = new Dictionary<string, AuditLog>(StringComparer.OrdinalIgnoreCase);
        foreach (var log in logs.Where(log => NormalizeHash(log.PreviousHash) is not null))
        {
            var previousHash = NormalizeHash(log.PreviousHash)!;
            if (!byHash.ContainsKey(previousHash))
                return Failure(0, verifiedAt, "AUDIT_PREVIOUS_NOT_FOUND", "Un registro apunta a un hash anterior inexistente.", log.Id);
            if (!childrenByPreviousHash.TryAdd(previousHash, log))
                return Failure(0, verifiedAt, "AUDIT_CHAIN_FORK", "La cadena contiene una bifurcación.", log.Id);
        }

        var current = roots[0];
        var visited = new HashSet<Guid>();
        while (true)
        {
            if (!visited.Add(current.Id))
                return Failure(visited.Count, verifiedAt, "AUDIT_CHAIN_CYCLE", "La cadena contiene un ciclo.", current.Id);

            var currentHash = NormalizeHash(current.Hash)!;
            if (!childrenByPreviousHash.TryGetValue(currentHash, out var next))
                break;
            current = next;
        }

        if (visited.Count != logs.Count)
            return Failure(visited.Count, verifiedAt, "AUDIT_CHAIN_DISCONNECTED", "La cadena contiene registros desconectados.", null);

        return Success(visited.Count, verifiedAt);
    }

    public static string ComputeHash(AuditLog log)
    {
        var timestamp = NormalizeForPostgreSql(log.Timestamp);
        return ComputeHash(log, timestamp);
    }

    private static bool MatchesStoredHash(AuditLog log, string storedHash)
    {
        var persistedTimestamp = NormalizeForPostgreSql(log.Timestamp);
        if (string.Equals(ComputeHash(log, persistedTimestamp), storedHash, StringComparison.OrdinalIgnoreCase))
            return true;

        // PostgreSQL timestamp has microsecond precision. Versions anteriores calculaban
        // el hash antes de persistir y podían perder el séptimo dígito de fracción.
        for (var lostTick = 1; lostTick <= 9; lostTick++)
        {
            if (string.Equals(
                ComputeHash(log, persistedTimestamp.AddTicks(lostTick)),
                storedHash,
                StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private static string ComputeHash(AuditLog log, DateTime timestampValue)
    {
        var timestamp = timestampValue.ToString("O", CultureInfo.InvariantCulture);
        var canonical = $"{NormalizeHash(log.PreviousHash)}|{log.UserId}|{log.Action}|{log.EntityName}|{log.EntityId}|{log.CorrelativeNumber}|{log.PaymentMethod}|{log.Changes}|{timestamp}";
        return Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(canonical)));
    }

    public static byte[] SerializeExport(AuditChainExport export) =>
        JsonSerializer.SerializeToUtf8Bytes(export, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

    private static IReadOnlyList<AuditLog> OrderChain(IReadOnlyCollection<AuditLog> logs)
    {
        if (logs.Count == 0) return [];
        var byPrevious = logs
            .Where(log => NormalizeHash(log.PreviousHash) is not null)
            .GroupBy(log => NormalizeHash(log.PreviousHash)!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        var root = logs.FirstOrDefault(log => NormalizeHash(log.PreviousHash) is null);
        if (root is null) return logs.OrderBy(log => log.Timestamp).ThenBy(log => log.Id).ToList();

        var ordered = new List<AuditLog>();
        var visited = new HashSet<Guid>();
        var current = root;
        while (visited.Add(current.Id))
        {
            ordered.Add(current);
            var hash = NormalizeHash(current.Hash);
            if (hash is null || !byPrevious.TryGetValue(hash, out var next)) break;
            current = next;
        }

        ordered.AddRange(logs.Where(log => !visited.Contains(log.Id)).OrderBy(log => log.Timestamp).ThenBy(log => log.Id));
        return ordered;
    }

    private static string? NormalizeHash(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();

    private static DateTime NormalizeUtc(DateTime value) =>
        value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);

    private static DateTime NormalizeForPostgreSql(DateTime value)
    {
        var utc = NormalizeUtc(value);
        return utc.AddTicks(-(utc.Ticks % 10));
    }

    private static AuditIntegrityResult Success(int count, DateTime verifiedAt) =>
        new(true, count, NormalizeUtc(verifiedAt));

    private static AuditIntegrityResult Failure(int count, DateTime verifiedAt, string code, string message, Guid? id) =>
        new(false, count, NormalizeUtc(verifiedAt), code, message, id);
}
