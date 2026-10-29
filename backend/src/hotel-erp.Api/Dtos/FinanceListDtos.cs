using System.ComponentModel.DataAnnotations;

namespace hotel_erp.Api.Dtos.Common;

public sealed class CashMovementPageQuery : SearchPageQuery
{
    [RegularExpression("^(Apertura|Cierre|Ingreso|Egreso|Arqueo)$")]
    public string? MovementType { get; set; }
}

public sealed class BackupPageQuery : SearchPageQuery
{
    [RegularExpression("^(CreatedLocal|PendingUpload|UploadedToDrive|UploadFailed|Failed)$")]
    public string? Status { get; set; }
}

public sealed record AccountingEntryListDto
{
    public Guid Id { get; init; }
    public DateOnly TransactionDate { get; init; }
    public string Description { get; init; } = string.Empty;
    public string EntryType { get; init; } = string.Empty;
    public decimal TotalDebit { get; init; }
    public decimal TotalCredit { get; init; }
    public int ItemCount { get; init; }
}

public sealed record JournalSummaryDto(decimal Debit, decimal Credit);
public sealed record EligibleCardSummaryDto(int TotalPayments, decimal AvailableAmount);

public sealed record BackupLogDto
{
    public Guid Id { get; init; }
    public DateTime StartedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public string FileName { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public string? Sha256Hash { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime? UploadedAt { get; init; }
    public int UploadAttempts { get; init; }
    public string? ErrorMessage { get; init; }
}
