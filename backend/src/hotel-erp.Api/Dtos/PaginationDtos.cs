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

public class SearchPageQuery : PageQuery
{
    [StringLength(100)]
    public string? Search { get; set; }
}

public sealed class ReservationPageQuery : SearchPageQuery
{
    [RegularExpression("^(Pendiente|Confirmada|CheckIn|CheckOut|Cancelada)$")]
    public string? Status { get; set; }
}

public sealed class FolioPageQuery : SearchPageQuery
{
    [RegularExpression("^(Abierto|Cerrado|PendientePago)$")]
    public string? Status { get; set; }
}

public sealed class ProductPageQuery : SearchPageQuery
{
    public Guid? CategoryId { get; set; }
    public bool? IsActive { get; set; }
}

public sealed class InventoryMovementPageQuery : SearchPageQuery
{
    public Guid? ProductId { get; set; }
    [RegularExpression("^(Entrada|Salida|Ajuste|Compra|Venta)$")]
    public string? MovementType { get; set; }
}

public sealed record FolioListDto
{
    public Guid Id { get; init; }
    public string GuestName { get; init; } = string.Empty;
    public string RoomNumber { get; init; } = string.Empty;
    public DateTime OpeningDate { get; init; }
    public DateTime? ClosingDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public int ItemCount { get; init; }
}

public sealed record InventorySummaryDto(int TotalProducts, int ActiveProducts, int LowStockProducts, int TotalMovements);
