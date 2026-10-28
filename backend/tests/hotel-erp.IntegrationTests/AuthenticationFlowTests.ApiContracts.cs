using System.Net;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using hotel_erp.Api.Services.Interfaces;
using System.Text;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static async Task AssertApiContractAndPaginationAsync(
        HttpClient client, IServiceProvider services, string accessToken)
    {
        const string marker = "Paginación QA";
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Guests.AddRange(Enumerable.Range(0, 37).Select(index => new Guest
            {
                FirstName = marker,
                LastName = "Mismo apellido",
                Company = $"Grupo {index:D2}"
            }));
            await context.SaveChangesAsync();
        }
        var first = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            $"/api/guests/paged?search={Uri.EscapeDataString(marker)}&page=1&pageSize=10", null, HttpStatusCode.OK);
        var second = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            $"/api/guests/paged?search={Uri.EscapeDataString(marker)}&page=2&pageSize=10", null, HttpStatusCode.OK);
        Assert.Equal(37, first.GetProperty("totalItems").GetInt32());
        Assert.Equal(4, first.GetProperty("totalPages").GetInt32());
        Assert.Equal(10, first.GetProperty("items").GetArrayLength());
        var legacy = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            $"/api/guests?search={Uri.EscapeDataString(marker)}", null, HttpStatusCode.OK);
        Assert.Equal(37, legacy.GetArrayLength());
        var firstIds = first.GetProperty("items").EnumerateArray().Select(item => item.GetProperty("id").GetGuid()).ToHashSet();
        Assert.DoesNotContain(second.GetProperty("items").EnumerateArray(), item => firstIds.Contains(item.GetProperty("id").GetGuid()));
        var last = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            $"/api/guests/paged?search={Uri.EscapeDataString(marker)}&page=100&pageSize=10", null, HttpStatusCode.OK);
        Assert.Equal(4, last.GetProperty("page").GetInt32());
        Assert.Equal(7, last.GetProperty("items").GetArrayLength());
        var empty = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            "/api/guests/paged?search=no-existing-synthetic-record", null, HttpStatusCode.OK);
        Assert.Equal(0, empty.GetProperty("totalItems").GetInt32());

        using (var request = Authorized(HttpMethod.Get, "/api/guests/paged?pageSize=101", accessToken))
        using (var response = await client.SendAsync(request))
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.BadRequest);
        using (var request = Authorized(HttpMethod.Get, "/api/guests/paged?page=0", accessToken))
        using (var response = await client.SendAsync(request))
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.BadRequest);

        DateOnly date;
        int expectedInvoices;
        Guid caiId;
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var invoice = await context.Invoices.FirstAsync();
            date = DateOnly.FromDateTime(invoice.InvoiceDate);
            caiId = invoice.CAIId;
            var begin = date.ToDateTime(TimeOnly.MinValue);
            var end = date.AddDays(1).ToDateTime(TimeOnly.MinValue);
            expectedInvoices = await context.Invoices.CountAsync(candidate => candidate.CAIId == caiId
                && candidate.DocumentType == InvoiceDocumentType.Factura
                && candidate.InvoiceDate >= begin && candidate.InvoiceDate < end);
        }
        var invoices = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            $"/api/invoices/paged?start={date:yyyy-MM-dd}&end={date:yyyy-MM-dd}&caiId={caiId}&documentType=Factura&pageSize=100",
            null, HttpStatusCode.OK);
        Assert.Equal(expectedInvoices, invoices.GetProperty("totalItems").GetInt32());
        Assert.True(expectedInvoices > 0);
        Assert.All(invoices.GetProperty("items").EnumerateArray(), item => Assert.Equal("Factura", item.GetProperty("documentType").GetString()));
        using (var request = Authorized(HttpMethod.Get, "/api/invoices/paged?start=2026-10-02&end=2026-10-01", accessToken))
        using (var response = await client.SendAsync(request))
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.BadRequest);

        int expectedAudit;
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var service = scope.ServiceProvider.GetRequiredService<IAuditService>();
            await using var transaction = await context.Database.BeginTransactionAsync();
            for (var index = 0; index < 210; index++)
                await service.LogAsync(null, "PaginationAuditProof", "Synthetic", null, new { Index = index });
            await transaction.CommitAsync();
            expectedAudit = await context.AuditLogs.CountAsync();
        }
        Assert.True(expectedAudit > 200);
        var audit = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            "/api/audit-logs/paged?pageSize=1", null, HttpStatusCode.OK);
        Assert.Equal(expectedAudit, audit.GetProperty("totalItems").GetInt32());
        Assert.Single(audit.GetProperty("items").EnumerateArray());
        var actions = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            "/api/audit-logs/actions", null, HttpStatusCode.OK);
        Assert.Contains(actions.EnumerateArray(), action => action.GetString() == "CreateInvoice");

        using var documentResponse = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.OK, documentResponse.StatusCode);
        var documentText = await documentResponse.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(documentText);
        var root = document.RootElement;
        var operation = root.GetProperty("paths").GetProperty("/api/Guests/paged").GetProperty("get");
        Assert.Equal("Guests_GetPage", operation.GetProperty("operationId").GetString());
        Assert.True(operation.TryGetProperty("security", out _));
        Assert.True(operation.GetProperty("responses").TryGetProperty("401", out _));
        Assert.True(operation.GetProperty("responses").GetProperty("400").GetProperty("content")
            .TryGetProperty("application/problem+json", out _));
        Assert.Equal("http", root.GetProperty("components").GetProperty("securitySchemes")
            .GetProperty("Bearer").GetProperty("type").GetString());
        var schemaPath = Environment.GetEnvironmentVariable("HOTEL_ERP_OPENAPI_OUTPUT");
        if (!string.IsNullOrWhiteSpace(schemaPath)) await File.WriteAllTextAsync(schemaPath, documentText);
        var metricsPath = Environment.GetEnvironmentVariable("HOTEL_ERP_PAGINATION_METRICS");
        if (!string.IsNullOrWhiteSpace(metricsPath))
            await File.WriteAllTextAsync(metricsPath, JsonSerializer.Serialize(new
            {
                containsSecretValues = false,
                guestMatchCount = 37,
                guestLegacyRows = legacy.GetArrayLength(),
                guestPageRows = first.GetProperty("items").GetArrayLength(),
                guestLegacyPayloadBytes = Encoding.UTF8.GetByteCount(legacy.GetRawText()),
                guestPagePayloadBytes = Encoding.UTF8.GetByteCount(first.GetRawText()),
                auditTotalItems = expectedAudit,
                auditPageRows = audit.GetProperty("items").GetArrayLength()
            }, new JsonSerializerOptions { WriteIndented = true }));
    }
}
