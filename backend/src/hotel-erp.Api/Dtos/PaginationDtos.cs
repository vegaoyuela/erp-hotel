using System.ComponentModel.DataAnnotations;

namespace hotel_erp.Api.Dtos.Common;

public class PageQuery
{
    [Range(1, 1_000_000)]
    public int Page { get; set; } = 1;
    [Range(1, 100)]
    public int PageSize { get; set; } = 25;
}

public sealed class GuestPageQuery : PageQuery
{
    [StringLength(100)]
    public string? Search { get; set; }
}

public sealed class InvoicePageQuery : PageQuery, IValidatableObject
{
    [StringLength(50)]
    public string? Dni { get; set; }
    public DateOnly? Start { get; set; }
    public DateOnly? End { get; set; }
    public Guid? CaiId { get; set; }
    public Guid? DocumentAuthorizationId { get; set; }
    public Guid? OriginalInvoiceId { get; set; }
    [RegularExpression("^(Factura|NotaCredito|NotaDebito)$")]
    public string? DocumentType { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Start.HasValue && End.HasValue && End < Start)
            yield return new("La fecha final debe ser igual o posterior a la inicial", [nameof(End)]);
    }
}

public sealed class AuditPageQuery : PageQuery
{
    [StringLength(100)]
    public string? Search { get; set; }
    [StringLength(100)]
    public string? Action { get; set; }
}

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalItems, int TotalPages);
