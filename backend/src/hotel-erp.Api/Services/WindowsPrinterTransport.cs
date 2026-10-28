using hotel_erp.Api.Services.Interfaces;

namespace hotel_erp.Api.Services;

public sealed class WindowsPrinterTransport : IPrinterTransport
{
    public bool IsSupported => OperatingSystem.IsWindows();

    public string[] GetInstalledPrinters() => RawPrinterHelper.GetInstalledPrinters();

    public (bool Success, string Message) TestPrinterConnection(string printerName)
        => RawPrinterHelper.TestPrinterConnection(printerName);

    public void Send(string printerName, byte[] data)
    {
        if (!RawPrinterHelper.SendBytesToPrinter(printerName, data))
            throw new InvalidOperationException("El servicio de impresión no aceptó el trabajo.");
    }
}
