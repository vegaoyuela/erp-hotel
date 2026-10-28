namespace hotel_erp.Api.Services.Interfaces;

public interface IAuditService
{
    Task LogAsync(Guid? userId, string action, string? entityName, Guid? entityId,
        object? changes = null, string? correlativeNumber = null, string? paymentMethod = null);
}
