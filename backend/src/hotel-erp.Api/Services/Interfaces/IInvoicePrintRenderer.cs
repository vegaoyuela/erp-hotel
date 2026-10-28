using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services.Interfaces;

public interface IInvoicePrintRenderer
{
    List<string> BuildLines(Invoice invoice, BusinessSettings settings, string? guestName,
        string? checkIn, string? checkOut, string tipoLabel, bool includePago);
    string GeneratePreviewText(Invoice invoice, BusinessSettings settings, string? guestName,
        string? checkIn, string? checkOut);
    byte[] GenerateTwoCopyInvoice(Invoice invoice, BusinessSettings settings, string? guestName,
        string? checkIn, string? checkOut);
    byte[] BuildRulerData(BusinessSettings settings, int rulerWidth = 100);
}
