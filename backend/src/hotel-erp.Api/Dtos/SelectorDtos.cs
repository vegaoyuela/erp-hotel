using System.ComponentModel.DataAnnotations;

namespace hotel_erp.Api.Dtos.Common;

public sealed class CashRegisterPageQuery : SearchPageQuery
{
    public bool OpenOnly { get; set; }
}

public sealed class AuthorizationPageQuery : SearchPageQuery
{
    [RegularExpression("^(cai|docauth)$")]
    public string? Source { get; set; }
    [RegularExpression("^(Factura|NotaCredito|NotaDebito)$")]
    public string? DocumentType { get; set; }
    public bool ActiveOnly { get; set; }
}

public sealed record AuthorizationOptionDto
{
    public Guid Id { get; init; }
    public string Source { get; init; } = string.Empty;
    public string DocumentType { get; init; } = string.Empty;
    public string CAINumber { get; init; } = string.Empty;
    public DateOnly IssueDate { get; init; }
    public DateOnly DueDate { get; init; }
    public string InitialRange { get; init; } = string.Empty;
    public string FinalRange { get; init; } = string.Empty;
    public string CurrentCorrelative { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsExpiringSoon { get; init; }
    public bool HasAttachment { get; init; }
}
