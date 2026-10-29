using System.Net;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static async Task AssertSelectorContractsAsync(HttpClient client, IServiceProvider services, string token)
    {
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var today = scope.ServiceProvider.GetRequiredService<IHotelClock>().Today;
            var user = await context.Users.FirstAsync();
            for (var index = 0; index < 31; index++)
            {
                context.CAIs.Add(new CAI
                {
                    CAINumber = $"SEL-QA-CAI-{index:D2}",
                    IssueDate = today.AddDays(-1),
                    DueDate = today.AddDays(60),
                    InitialRange = "001-001-01-00000001",
                    FinalRange = "001-001-01-00000100",
                    CurrentCorrelative = "001-001-01-00000001"
                });
                context.DocumentAuthorizations.Add(new DocumentAuthorization
                {
                    CAINumber = $"SEL-QA-DOC-{index:D2}",
                    DocumentType = index % 2 == 0 ? InvoiceDocumentType.NotaCredito : InvoiceDocumentType.NotaDebito,
                    IssueDate = today.AddDays(-1),
                    DueDate = today.AddDays(60),
                    InitialRange = "002-001-03-00000001",
                    FinalRange = "002-001-03-00000100",
                    CurrentCorrelative = "002-001-03-00000001"
                });
                var open = new CashRegister { Name = $"SEL-QA-Open-{index:D2}" };
                context.CashMovements.Add(new CashMovement
                {
                    CashRegister = open,
                    User = user,
                    MovementType = CashMovementType.Apertura,
                    Amount = 100m,
                    BalanceAfter = 100m
                });
                context.CashRegisters.Add(new CashRegister { Name = $"SEL-QA-Closed-{index:D2}" });
            }
            context.CashMovements.Add(new CashMovement
            {
                CashRegister = new CashRegister { Name = "SEL-QA-Inactive", IsActive = false },
                User = user,
                MovementType = CashMovementType.Apertura,
                Amount = 100m,
                BalanceAfter = 100m
            });
            foreach (var (name, start, end, status) in new[]
            {
                ("Expired", today.AddDays(-60), today.AddDays(-1), CAIStatus.Activo),
                ("Future", today.AddDays(1), today.AddDays(90), CAIStatus.Activo),
                ("Disabled", today.AddDays(-1), today.AddDays(60), CAIStatus.Desactivado)
            }) context.CAIs.Add(new CAI
            {
                CAINumber = $"SEL-QA-{name}",
                IssueDate = start,
                DueDate = end,
                Status = status,
                InitialRange = "001-001-01-00000001",
                FinalRange = "001-001-01-00000100",
                CurrentCorrelative = "001-001-01-00000001"
            });
            await context.SaveChangesAsync();
        }
        async Task<JsonElement> Read(string path) => await SendPersistenceJsonAsync(client, token, HttpMethod.Get, path, null, HttpStatusCode.OK);
        var all = await Read("/api/document-authorizations/options/paged?search=SEL-QA&pageSize=100");
        Assert.Equal(65, all.GetProperty("totalItems").GetInt32());
        var active = await Read("/api/document-authorizations/options/paged?search=SEL-QA&activeOnly=true&pageSize=10");
        Assert.Equal(62, active.GetProperty("totalItems").GetInt32());
        var next = await Read("/api/document-authorizations/options/paged?search=SEL-QA&activeOnly=true&pageSize=10&page=2");
        var ids = active.GetProperty("items").EnumerateArray().Select(row => row.GetProperty("source").GetString() + row.GetProperty("id").GetString()).ToHashSet();
        Assert.DoesNotContain(next.GetProperty("items").EnumerateArray(), row => ids.Contains(row.GetProperty("source").GetString() + row.GetProperty("id").GetString()));
        var cai = await Read("/api/document-authorizations/options/paged?search=SEL-QA&source=cai&activeOnly=true");
        Assert.Equal(31, cai.GetProperty("totalItems").GetInt32());
        var credit = await Read("/api/document-authorizations/options/paged?search=SEL-QA&source=docauth&documentType=NotaCredito&activeOnly=true");
        Assert.Equal(16, credit.GetProperty("totalItems").GetInt32());
        var cash = await Read("/api/cash-registers/paged?search=SEL-QA&openOnly=true&pageSize=100");
        Assert.Equal(31, cash.GetProperty("totalItems").GetInt32());
        Assert.All(cash.GetProperty("items").EnumerateArray(), row => Assert.True(row.GetProperty("isOpen").GetBoolean() && row.GetProperty("isActive").GetBoolean()));
        foreach (var path in new[] { "/api/document-authorizations/options/paged?source=invalid", "/api/document-authorizations/options/paged?documentType=99", "/api/cash-registers/paged?openOnly=invalid" })
        {
            using var request = Authorized(HttpMethod.Get, path, token);
            using var response = await client.SendAsync(request);
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.BadRequest);
        }
        using var documentResponse = await client.GetAsync("/openapi/v1.json");
        using var document = JsonDocument.Parse(await documentResponse.Content.ReadAsStringAsync());
        var paths = document.RootElement.GetProperty("paths");
        var missing = new List<string>();
        var binaryResponses = 0;
        foreach (var path in paths.EnumerateObject())
            foreach (var operation in path.Value.EnumerateObject())
            {
                if (!operation.Value.TryGetProperty("responses", out var responses)) continue;
                foreach (var response in responses.EnumerateObject().Where(row => row.Name is "200" or "201"))
                {
                    if (!response.Value.TryGetProperty("content", out var content) || !content.EnumerateObject().Any(media => media.Value.TryGetProperty("schema", out _)))
                        missing.Add(operation.Name + " " + path.Name);
                    else binaryResponses += content.EnumerateObject().Count(media => media.Value.GetProperty("schema").TryGetProperty("format", out var format) && format.GetString() == "binary");
                }
            }
        Assert.Empty(missing);
        Assert.True(binaryResponses >= 8);
        Assert.True(paths.GetProperty("/api/Guests/{id}").GetProperty("put").GetProperty("responses").TryGetProperty("204", out _));
        Assert.False(paths.GetProperty("/api/Invoices/{id}/cancel").GetProperty("post").GetProperty("responses").TryGetProperty("200", out _));
        var output = Environment.GetEnvironmentVariable("HOTEL_ERP_SELECTOR_METRICS");
        if (!string.IsNullOrWhiteSpace(output)) await File.WriteAllTextAsync(output, JsonSerializer.Serialize(new
        {
            syntheticDatabase = true,
            unifiedAuthorizations = 65,
            activeAuthorizations = 62,
            activeLegacyCais = 31,
            creditAuthorizations = 16,
            eligibleCashRegisters = 31,
            successfulResponsesWithoutSchema = missing.Count,
            binaryResponses
        }, new JsonSerializerOptions { WriteIndented = true }));
    }
}
