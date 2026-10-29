namespace hotel_erp.Api.Infrastructure;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ApiFileResponseAttribute(string contentType) : Attribute
{
    public string ContentType { get; } = contentType;
}
