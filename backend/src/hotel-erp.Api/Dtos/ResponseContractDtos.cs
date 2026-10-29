namespace hotel_erp.Api.Dtos.Common;

public sealed record MessageResponseDto(string Message);
public sealed record GuestClassificationDto(string? Classification);
public sealed record PrinterConnectionDto(bool Success, string Message);
public sealed record PrintedInvoiceDto(string Message, string Printer);
public sealed record PrintPreviewDto(string Text, string? LogoBase64, int PrintLogoHeight, int PrintWidth);
public sealed record HealthResponseDto(string Status, DateTime Timestamp);
public sealed record CurrentUserDto(string? Id, string? Username, string? Email, string? FullName,
    IReadOnlyList<string> Roles, IReadOnlyList<string> Permissions, bool MustChangePassword);
