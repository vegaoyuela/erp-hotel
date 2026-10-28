using System.Net;
using System.Text;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static async Task AssertReplaceableInvoiceContractsAsync(
        string connectionString, IServiceProvider services, string accessToken, Guid caiId)
    {
        using (var firstScope = services.CreateScope())
        using (var secondScope = services.CreateScope())
        {
            var firstTax = firstScope.ServiceProvider.GetRequiredService<ITaxCalculator>();
            var secondTax = secondScope.ServiceProvider.GetRequiredService<ITaxCalculator>();
            Assert.NotSame(firstTax, secondTax);
            firstTax.IsvRate = 0.18m;
            Assert.Equal(0.15m, secondTax.IsvRate);
        }

        var before = await CaptureContractStateAsync(services, caiId);
        var calculator = new RejectingInvoiceCalculator();
        await using (var factory = new HotelErpWebApplicationFactory(connectionString, collection =>
        {
            collection.RemoveAll<IInvoiceCalculator>();
            collection.AddSingleton<IInvoiceCalculator>(calculator);
        }))
        using (var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false }))
        using (var request = InvoiceRequest(accessToken, caiId, Guid.NewGuid().ToString("D")))
        using (var response = await client.SendAsync(request))
        {
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.InternalServerError);
            Assert.Equal(1, calculator.Calls);
        }
        Assert.Equal(before, await CaptureContractStateAsync(services, caiId));

        var audit = new RejectingInvoiceAudit();
        await using (var factory = new HotelErpWebApplicationFactory(connectionString, collection =>
        {
            collection.RemoveAll<IAuditService>();
            collection.AddSingleton<IAuditService>(audit);
        }))
        using (var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false }))
        using (var request = InvoiceRequest(accessToken, caiId, Guid.NewGuid().ToString("D")))
        using (var response = await client.SendAsync(request))
        {
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.InternalServerError);
            Assert.Equal(1, audit.InvoiceCalls);
        }
        Assert.Equal(before, await CaptureContractStateAsync(services, caiId));
    }

    private static async Task<ContractState> CaptureContractStateAsync(IServiceProvider services, Guid caiId)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return new(
            await context.Invoices.CountAsync(),
            await context.InvoiceItems.CountAsync(),
            await context.Payments.CountAsync(),
            await context.CashMovements.CountAsync(),
            await context.AccountingEntries.CountAsync(),
            await context.AccountingAccounts.CountAsync(),
            await context.AuditLogs.CountAsync(),
            await context.IdempotencyRecords.CountAsync(),
            await context.CAIs.Where(cai => cai.Id == caiId).Select(cai => cai.CurrentCorrelative).SingleAsync());
    }

    private static async Task AssertPrinterContractsAsync(
        string connectionString, IServiceProvider services, string accessToken, Guid invoiceId)
    {
        string? originalPrinter;
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var settings = await context.BusinessSettings.FirstAsync();
            originalPrinter = settings.PrintPrinterName;
            settings.PrintPrinterName = "QA-Virtual";
            await context.SaveChangesAsync();
        }

        try
        {
            var transport = new RecordingPrinterTransport();
            var renderer = new RecordingPrintRenderer();
            await using var factory = new HotelErpWebApplicationFactory(connectionString, collection =>
            {
                collection.RemoveAll<IPrinterTransport>();
                collection.RemoveAll<IInvoicePrintRenderer>();
                collection.AddSingleton<IPrinterTransport>(transport);
                collection.AddSingleton<IInvoicePrintRenderer>(renderer);
            });
            using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            var printers = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
                "/api/print/printers", null, HttpStatusCode.OK);
            Assert.Equal("QA-Virtual", Assert.Single(printers.EnumerateArray()).GetString());
            var connection = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
                "/api/print/test?name=QA-Virtual", null, HttpStatusCode.OK);
            Assert.True(connection.GetProperty("success").GetBoolean());

            await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
                "/api/print/test-preview", null, HttpStatusCode.OK);
            Assert.Empty(transport.Jobs);
            var sample = renderer.LastPreviewInvoice!;
            Assert.Equal(300m, sample.SubTotal);
            Assert.Equal(45m, sample.ISV15Amount);
            Assert.Equal(12m, sample.TouristTaxAmount);
            Assert.Equal(357m, sample.TotalAmount);
            Assert.Equal(sample.SubTotal, sample.InvoiceItems.Sum(item => item.LineTotal));
            Assert.Equal(sample.TotalAmount, sample.SubTotal + sample.ISVAmount + sample.TouristTaxAmount);

            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Post,
                "/api/print/test-print", null, HttpStatusCode.OK);
            var printedSample = Assert.Single(transport.Jobs);
            Assert.Contains("ORIGINAL: CLIENTE", Encoding.UTF8.GetString(printedSample));
            Assert.Contains("COPIA: OBLIGADO TRIBUTARIO EMISOR", Encoding.UTF8.GetString(printedSample));

            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Post,
                "/api/print/test-ruler", null, HttpStatusCode.OK);
            Assert.Equal(1, renderer.RulerCalls);
            Assert.Equal(2, transport.Jobs.Count);

            await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
                $"/api/print/invoice/{invoiceId}/preview", null, HttpStatusCode.OK);
            Assert.Equal(invoiceId, renderer.LastPreviewInvoice!.Id);
            Assert.Equal(2, transport.Jobs.Count);

            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Post,
                $"/api/print/invoice/{invoiceId}", null, HttpStatusCode.OK);
            Assert.Equal(3, transport.Jobs.Count);
            Assert.All(transport.PrinterNames, name => Assert.Equal("QA-Virtual", name));
            Assert.Equal(2, renderer.InvoiceRenderCalls);

            transport.ThrowOnSend = true;
            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Post,
                $"/api/print/invoice/{invoiceId}", null, HttpStatusCode.BadRequest);
            transport.ThrowOnSend = false;
            renderer.ThrowOnRender = true;
            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Post,
                $"/api/print/invoice/{invoiceId}", null, HttpStatusCode.BadRequest);
            Assert.Equal(3, transport.Jobs.Count);

            transport.IsSupported = false;
            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Post,
                $"/api/print/invoice/{invoiceId}", null, HttpStatusCode.NotImplemented);
            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Get,
                "/api/print/printers", null, HttpStatusCode.NotImplemented);
            Assert.Equal(3, transport.Jobs.Count);
        }
        finally
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            (await context.BusinessSettings.FirstAsync()).PrintPrinterName = originalPrinter;
            await context.SaveChangesAsync();
        }
    }

    private sealed record ContractState(int Invoices, int Items, int Payments, int CashMovements,
        int Entries, int Accounts, int Audits, int Idempotency, string Correlative);

    private sealed class RejectingInvoiceCalculator : IInvoiceCalculator
    {
        public int Calls { get; private set; }
        public InvoiceCalculation Calculate(IEnumerable<InvoiceLineInput> source,
            bool isIsvExempt = false, bool isTouristTaxExempt = false)
        {
            Calls++;
            throw new InvalidOperationException("synthetic-application-failure");
        }
    }

    private sealed class RejectingInvoiceAudit : IAuditService
    {
        public int InvoiceCalls { get; private set; }
        public Task LogAsync(Guid? userId, string action, string? entityName, Guid? entityId,
            object? changes = null, string? correlativeNumber = null, string? paymentMethod = null)
        {
            if (action != "CreateInvoice") return Task.CompletedTask;
            InvoiceCalls++;
            throw new InvalidOperationException("synthetic-application-failure");
        }
    }

    private sealed class RecordingPrinterTransport : IPrinterTransport
    {
        public bool IsSupported { get; set; } = true;
        public bool ThrowOnSend { get; set; }
        public List<byte[]> Jobs { get; } = [];
        public List<string> PrinterNames { get; } = [];
        public string[] GetInstalledPrinters() => ["QA-Virtual"];
        public (bool Success, string Message) TestPrinterConnection(string printerName) => (true, "QA");
        public void Send(string printerName, byte[] data)
        {
            if (ThrowOnSend) throw new InvalidOperationException("synthetic-printer-failure");
            PrinterNames.Add(printerName);
            Jobs.Add(data.ToArray());
        }
    }

    private sealed class RecordingPrintRenderer : IInvoicePrintRenderer
    {
        private readonly EscPosService _renderer = new();
        public Invoice? LastPreviewInvoice { get; private set; }
        public int InvoiceRenderCalls { get; private set; }
        public int RulerCalls { get; private set; }
        public bool ThrowOnRender { get; set; }
        public List<string> BuildLines(Invoice invoice, BusinessSettings settings, string? guestName,
            string? checkIn, string? checkOut, string tipoLabel, bool includePago)
            => _renderer.BuildLines(invoice, settings, guestName, checkIn, checkOut, tipoLabel, includePago);
        public string GeneratePreviewText(Invoice invoice, BusinessSettings settings, string? guestName,
            string? checkIn, string? checkOut)
        {
            LastPreviewInvoice = invoice;
            return _renderer.GeneratePreviewText(invoice, settings, guestName, checkIn, checkOut);
        }
        public byte[] GenerateTwoCopyInvoice(Invoice invoice, BusinessSettings settings, string? guestName,
            string? checkIn, string? checkOut)
        {
            if (ThrowOnRender) throw new InvalidOperationException("synthetic-renderer-failure");
            InvoiceRenderCalls++;
            return _renderer.GenerateTwoCopyInvoice(invoice, settings, guestName, checkIn, checkOut);
        }
        public byte[] BuildRulerData(BusinessSettings settings, int rulerWidth = 100)
        {
            RulerCalls++;
            return _renderer.BuildRulerData(settings, rulerWidth);
        }
    }
}
