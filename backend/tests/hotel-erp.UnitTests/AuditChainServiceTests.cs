using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;

namespace hotel_erp.UnitTests;

public class AuditChainServiceTests
{
    [Fact]
    public void Verify_AcceptsLinkedChainRegardlessOfInputOrder()
    {
        var first = CreateLog("Login", null, new DateTime(2026, 9, 27, 12, 0, 0, DateTimeKind.Utc));
        var second = CreateLog("CreateInvoice", first.Hash, first.Timestamp.AddTicks(1));
        var third = CreateLog("CreatePayment", second.Hash, second.Timestamp);

        var result = AuditChainService.Verify([third, first, second], first.Timestamp.AddHours(1));

        Assert.True(result.IsValid);
        Assert.Equal(3, result.TotalRecordsVerified);
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public void Verify_DetectsContentTampering()
    {
        var log = CreateLog("CreateInvoice", null, new DateTime(2026, 9, 27, 12, 0, 0, DateTimeKind.Utc));
        log.Action = "DeleteInvoice";

        var result = AuditChainService.Verify([log], log.Timestamp.AddMinutes(1));

        Assert.False(result.IsValid);
        Assert.Equal("AUDIT_HASH_MISMATCH", result.ErrorCode);
        Assert.Equal(log.Id, result.BrokenLogId);
    }

    [Fact]
    public void Verify_DetectsForkEvenWhenEveryRecordHashIsValid()
    {
        var root = CreateLog("Root", null, new DateTime(2026, 9, 27, 12, 0, 0, DateTimeKind.Utc));
        var left = CreateLog("Left", root.Hash, root.Timestamp.AddSeconds(1));
        var right = CreateLog("Right", root.Hash, root.Timestamp.AddSeconds(2));

        var result = AuditChainService.Verify([root, left, right], root.Timestamp.AddMinutes(1));

        Assert.False(result.IsValid);
        Assert.Equal("AUDIT_CHAIN_FORK", result.ErrorCode);
    }

    [Fact]
    public void ComputeHash_IsStableAfterPostgreSqlReturnsUnspecifiedTimestamp()
    {
        var utc = CreateLog("Stable", null, new DateTime(2026, 9, 27, 12, 34, 56, 789, DateTimeKind.Utc));
        var expected = utc.Hash;
        utc.Timestamp = DateTime.SpecifyKind(utc.Timestamp, DateTimeKind.Unspecified);

        Assert.Equal(expected, AuditChainService.ComputeHash(utc));
    }

    [Fact]
    public void Verify_AcceptsLegacyHashThatLostSubMicrosecondTickInPostgreSql()
    {
        var persisted = new DateTime(2026, 9, 27, 12, 34, 56, 789, DateTimeKind.Utc);
        var log = CreateLog("Legacy", null, persisted);
        var original = persisted.AddTicks(7);
        var input = $"|{log.UserId}|{log.Action}|{log.EntityName}|{log.EntityId}|||{log.Changes}|{original:O}";
        log.Hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(input)));
        log.Timestamp = DateTime.SpecifyKind(persisted, DateTimeKind.Unspecified);

        var result = AuditChainService.Verify([log], persisted.AddMinutes(1));

        Assert.True(result.IsValid, result.ErrorMessage);
    }

    private static AuditLog CreateLog(string action, string? previousHash, DateTime timestamp)
    {
        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Action = action,
            EntityName = "SyntheticEntity",
            EntityId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Changes = "{\"synthetic\":true}",
            Timestamp = timestamp,
            HondurasTimestamp = timestamp.AddHours(-6),
            PreviousHash = previousHash
        };
        log.Hash = AuditChainService.ComputeHash(log);
        return log;
    }
}
