namespace hotel_erp.Api.Services;

public sealed record CatalogProblem
{
    public string? Type { get; init; }
    public string? Title { get; init; }
    public int? Status { get; init; }
    public string? Detail { get; init; }
}

public sealed record CatalogMutationResult(int Status, object? Value = null, Guid? CreatedId = null)
{
    public static CatalogMutationResult Ok(object value) => new(StatusCodes.Status200OK, value);
    public static CatalogMutationResult Created(Guid id, object value) => new(StatusCodes.Status201Created, value, id);
    public static CatalogMutationResult NoContent() => new(StatusCodes.Status204NoContent);
    public static CatalogMutationResult BadRequest(object value) => new(StatusCodes.Status400BadRequest, value);
    public static CatalogMutationResult NotFound() => new(StatusCodes.Status404NotFound);
    public static CatalogMutationResult Conflict(CatalogProblem value) => new(StatusCodes.Status409Conflict, value);
}
