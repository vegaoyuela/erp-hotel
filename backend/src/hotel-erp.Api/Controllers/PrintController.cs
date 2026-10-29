using hotel_erp.Api.Dtos.Common;
using System.Text;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Services;
using hotel_erp.Api.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrintController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IBusinessSettingsRepository _settingsRepo;
        private readonly IInvoicePrintRenderer _escPosService;
        private readonly IHotelClock _clock;
        private readonly IPrinterTransport _printerTransport;
        private readonly IInvoiceCalculator _invoiceCalculator;

        public PrintController(
            IInvoiceRepository invoiceRepo,
            IBusinessSettingsRepository settingsRepo,
            IInvoicePrintRenderer escPosService,
            IPrinterTransport printerTransport,
            IInvoiceCalculator invoiceCalculator,
            IHotelClock clock)
        {
            _invoiceRepo = invoiceRepo;
            _settingsRepo = settingsRepo;
            _escPosService = escPosService;
            _clock = clock;
            _printerTransport = printerTransport;
            _invoiceCalculator = invoiceCalculator;
        }

        [HttpGet("printers")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(typeof(IReadOnlyList<string>), StatusCodes.Status200OK)]
        public ActionResult GetPrinters()
        {
            if (!_printerTransport.IsSupported) return PrinterPlatformUnavailable();
            var printers = _printerTransport.GetInstalledPrinters();
            return Ok(printers);
        }

        [HttpGet("test")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(typeof(PrinterConnectionDto), StatusCodes.Status200OK)]
        public ActionResult TestPrinter([FromQuery] string name)
        {
            if (!_printerTransport.IsSupported) return PrinterPlatformUnavailable();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Especifique el nombre de la impresora" });

            var (success, message) = _printerTransport.TestPrinterConnection(name);
            return Ok(new { success, message });
        }

        [HttpGet("test-preview")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(typeof(PrintPreviewDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTestPreview([FromQuery] int? width)
        {
            var settings = await _settingsRepo.GetAsync() ?? new BusinessSettings();
            if (width.HasValue) settings.PrintWidth = width.Value;
            var fake = BuildFakeInvoice();
            var text = _escPosService.GeneratePreviewText(fake, settings, "Juan Perez", "15/05/2026", "18/05/2026");
            var logo = settings.ShowLogo ? settings.LogoBase64 ?? "" : "";
            return Ok(new { text, logoBase64 = logo, printLogoHeight = settings.PrintLogoHeight, printWidth = settings.PrintWidth });
        }

        [HttpPost("test-print")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> PrintTest([FromQuery] int? width)
        {
            if (!_printerTransport.IsSupported) return PrinterPlatformUnavailable();
            var settings = await _settingsRepo.GetAsync();
            if (settings == null)
                return BadRequest(new { message = "Configure los datos del negocio" });
            if (string.IsNullOrWhiteSpace(settings.PrintPrinterName))
                return BadRequest(new { message = "Configure la impresora en Configuracion" });
            if (width.HasValue) settings.PrintWidth = width.Value;

            try
            {
                var fake = BuildFakeInvoice();
                var data = _escPosService.GenerateTwoCopyInvoice(fake, settings, "Juan Perez", "15/05/2026", "18/05/2026");
                _printerTransport.Send(settings.PrintPrinterName, data);
                return Ok(new { message = "Factura de prueba enviada a imprimir" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("test-ruler")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(typeof(MessageResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTestRuler()
        {
            if (!_printerTransport.IsSupported) return PrinterPlatformUnavailable();
            var settings = await _settingsRepo.GetAsync();
            if (settings == null || string.IsNullOrWhiteSpace(settings.PrintPrinterName))
                return BadRequest(new { message = "Configure la impresora primero" });
            try
            {
                var data = _escPosService.BuildRulerData(settings, 100);
                _printerTransport.Send(settings.PrintPrinterName, data);
                return Ok(new { message = "Regla de calibracion impresa. Observe hasta que numero llega el papel sin cortarse y anote ese valor en 'Ancho observado' en la pantalla de configuracion." });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        private Invoice BuildFakeInvoice()
        {
            var calculation = _invoiceCalculator.Calculate(
                [new InvoiceLineInput("Habitación Sencilla|Hospedaje x3 noche(s)", 3, 100m, false, 0.15m, true, 0m)]);
            const decimal cashReceived = 500m;
            return new Invoice
            {
                CAI = new CAI
                {
                    CAINumber = "A1B2-C3D4-E5F6-G7H8",
                    InitialRange = "001-001-01-00000001",
                    FinalRange = "001-001-01-00005000",
                    DueDate = _clock.Today.AddYears(2),
                    Status = CAIStatus.Activo
                },
                CorrelativeNumber = "001-001-01-00000001",
                InvoiceDate = _clock.HondurasNow,
                CustomerName = "Juan Perez - Factura de Prueba",
                RTNCliente = "08019012345678",
                SubTotal = calculation.SubTotal,
                ISVAmount = calculation.ISVAmount,
                ISV15Amount = calculation.ISV15Amount,
                ISV18Amount = calculation.ISV18Amount,
                TouristTaxAmount = calculation.TouristTaxAmount,
                DiscountsAmount = calculation.DiscountsAmount,
                TotalAmount = calculation.TotalAmount,
                PaymentMethod = "Efectivo",
                CashReceived = cashReceived,
                CashChange = cashReceived - calculation.TotalAmount,
                DocumentType = InvoiceDocumentType.Factura,
                Status = InvoiceStatus.Pagada,
                InvoiceItems = calculation.Items.ToList()
            };
        }

        [HttpGet("invoice/{id}/preview")]
        [ProducesResponseType(typeof(PrintPreviewDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPreview(Guid id)
        {
            var invoice = await _invoiceRepo.GetByIdAsync(id);
            if (invoice == null) return NotFound(new { message = "Factura no encontrada" });

            var settings = await _settingsRepo.GetAsync() ?? new BusinessSettings();

            string? guestName = null, checkIn = null, checkOut = null;
            if (invoice.Guest != null)
            {
                guestName = $"{invoice.Guest.FirstName} {invoice.Guest.LastName}";
                var res = invoice.Guest.Reservations.FirstOrDefault();
                if (res != null) { checkIn = res.CheckInDate.ToString("dd/MM/yyyy"); checkOut = res.CheckOutDate.ToString("dd/MM/yyyy"); }
            }

            var text = _escPosService.GeneratePreviewText(invoice, settings, guestName, checkIn, checkOut);
            var logo = settings.ShowLogo ? settings.LogoBase64 ?? "" : "";
            return Ok(new { text, logoBase64 = logo, printLogoHeight = settings.PrintLogoHeight, printWidth = settings.PrintWidth });
        }

        [HttpPost("invoice/{id}")]
        [ProducesResponseType(typeof(PrintedInvoiceDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> PrintInvoice(Guid id)
        {
            if (!_printerTransport.IsSupported) return PrinterPlatformUnavailable();
            var invoice = await _invoiceRepo.GetByIdAsync(id);
            if (invoice == null) return NotFound(new { message = "Factura no encontrada" });

            var settings = await _settingsRepo.GetAsync();
            if (settings == null)
                return BadRequest(new { message = "Configure primero los datos del negocio en Configuracion" });

            var printerName = settings.PrintPrinterName;
            if (string.IsNullOrWhiteSpace(printerName))
                return BadRequest(new { message = "Configure el nombre de la impresora en Configuracion" });

            string? guestName = null, checkIn = null, checkOut = null;

            if (invoice.Guest != null)
            {
                guestName = $"{invoice.Guest.FirstName} {invoice.Guest.LastName}";
                var res = invoice.Guest.Reservations.FirstOrDefault();
                if (res != null)
                {
                    checkIn = res.CheckInDate.ToString("dd/MM/yyyy");
                    checkOut = res.CheckOutDate.ToString("dd/MM/yyyy");
                }
            }

            try
            {
                var data = _escPosService.GenerateTwoCopyInvoice(invoice, settings, guestName, checkIn, checkOut);
                _printerTransport.Send(printerName, data);
                return Ok(new { message = "Factura enviada a imprimir", printer = printerName });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.Contains("StartDocPrinter"))
                    msg += ". Solucion: 1) Ejecute la terminal como Administrador. 2) Reinicie el servicio Spooler (PowerShell: Restart-Service Spooler).";
                return BadRequest(new { message = msg });
            }
        }

        private ObjectResult PrinterPlatformUnavailable()
            => StatusCode(StatusCodes.Status501NotImplemented, new
            {
                message = "La impresión ESC/POS directa requiere ejecutar el backend en Windows con acceso al servicio de impresión."
            });
    }
}
