namespace hotel_erp.Api.Services;

public sealed record ApplicationMutationResult<T>(
    T? Value,
    int? FailureStatus,
    string? FailureDetail)
    where T : class
{
    public bool Succeeded => Value is not null;

    public static ApplicationMutationResult<T> Success(T value) => new(value, null, null);
    public static ApplicationMutationResult<T> BadRequest(string detail) => new(null, StatusCodes.Status400BadRequest, detail);
    public static ApplicationMutationResult<T> NotFound(string detail) => new(null, StatusCodes.Status404NotFound, detail);
    public static ApplicationMutationResult<T> Conflict(string detail) => new(null, StatusCodes.Status409Conflict, detail);
}
