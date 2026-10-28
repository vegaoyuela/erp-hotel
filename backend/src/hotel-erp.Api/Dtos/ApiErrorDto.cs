namespace hotel_erp.Api.Dtos.Common;

/// <summary>Wire contract shared by ProblemDetails and field validation responses.</summary>
public sealed record ApiErrorDto(
    string Type, string Title, int Status, string Detail, string Instance,
    string Code, string TraceId, IReadOnlyDictionary<string, string[]>? Errors = null);
