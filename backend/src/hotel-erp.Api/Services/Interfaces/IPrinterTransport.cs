namespace hotel_erp.Api.Services.Interfaces;

/// <summary>Access to the printer host; rendering does not depend on this transport.</summary>
public interface IPrinterTransport
{
    bool IsSupported { get; }
    string[] GetInstalledPrinters();
    (bool Success, string Message) TestPrinterConnection(string printerName);
    void Send(string printerName, byte[] data);
}
