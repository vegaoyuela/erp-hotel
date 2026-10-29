using System.Net;
using System.Text;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static async Task AssertFinancePaginationAsync(HttpClient client, IServiceProvider services, string accessToken)
    {
        const string marker = "H2B-QA";
        Guid registerId;
        Guid partialPaymentId;
        Guid entryId;
        decimal expectedDebit;
        decimal expectedCredit;
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await context.Users.FirstAsync();
            var accounts = await context.AccountingAccounts.OrderBy(account => account.AccountNumber).Take(2).ToListAsync();
            var registers = Enumerable.Range(0, 31).Select(index => new CashRegister { Name = $"{marker} {index:D2}" }).ToList();
            context.CashRegisters.AddRange(registers);
            var cashMovements = new List<CashMovement>();
            for (var index = 0; index < 31; index++)
            {
                cashMovements.Add(new CashMovement
                {
                    CashRegister = registers[0],
                    User = user,
                    MovementType = index == 0 ? CashMovementType.Apertura : CashMovementType.Ingreso,
                    Amount = index == 0 ? 100m : 1m,
                    BalanceAfter = 100m + index,
                    Description = marker,
                    CreatedAt = new DateTime(2036, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(index),
                    MovementDate = new DateTime(2036, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(-index)
                });
            }
            context.CashMovements.AddRange(cashMovements);
            var entries = Enumerable.Range(0, 31).Select(index => new AccountingEntry
            {
                Description = $"{marker} asiento {index:D2}",
                TransactionDate = new DateOnly(2036, 1, 1),
                EntryItems = [new() { Account = accounts[0], Debit = 100.25m }, new() { Account = accounts[1], Credit = 100.25m }]
            }).ToList();
            context.AccountingEntries.AddRange(entries);
            context.BackupLogs.AddRange(Enumerable.Range(0, 31).Select(index => new BackupLog
            {
                FileName = $"{marker}-{index:D2}.dump",
                LocalPath = "/qa/hidden-local-path",
                GoogleDriveFileId = "private-drive-id",
                StartedAt = new DateTime(2036, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SizeBytes = 1024,
                Status = index % 2 == 0 ? BackupStatus.CreatedLocal : BackupStatus.Failed
            }));
            Payment NewPayment(string number, PaymentStatus status = PaymentStatus.Confirmado) => new()
            {
                PaymentNumber = number,
                ExternalReference = "POS " + number,
                Amount = 100m,
                RecordedByUser = user,
                Method = PaymentMethod.Tarjeta,
                Status = status,
                PaymentDate = new DateTime(2036, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
            var eligible = Enumerable.Range(0, 31).Select(index => NewPayment($"{marker}-ELIG-{index:D2}")).ToList();
            context.Payments.AddRange(eligible);
            for (var index = 0; index < 31; index++)
            {
                var payment = index == 0 ? eligible[0] : NewPayment($"{marker}-SETTLED-{index:D2}");
                var amount = index == 0 ? 20m : 100m;
                context.CardSettlements.Add(new CardSettlement
                {
                    SettlementNumber = $"{marker}-LIQ-{index:D2}",
                    ExternalReference = $"{marker}-BATCH-{index:D2}",
                    SettlementDate = new DateOnly(2036, 1, 1),
                    GrossAmount = amount,
                    BankDepositAmount = amount - 2m,
                    CommissionAmount = 2m,
                    RecordedByUser = user,
                    Applications = [new() { Payment = payment, Amount = amount }]
                });
            }
            var fullyRefunded = NewPayment("QA-fully-refunded", PaymentStatus.Reembolsado);
            context.Refunds.AddRange(new Refund
            {
                RefundNumber = "QA-H2B-refund",
                Payment = eligible[0],
                RecordedByUser = user,
                Method = PaymentMethod.Tarjeta,
                Amount = 10m,
                Reason = marker,
                ExternalReference = "QA-refund"
            }, new Refund
            {
                RefundNumber = "QA-H2B-void-refund",
                Payment = eligible[0],
                RecordedByUser = user,
                Method = PaymentMethod.Tarjeta,
                Amount = 5m,
                Status = RefundStatus.Anulado,
                Reason = marker,
                ExternalReference = "QA-void-refund"
            }, new Refund
            {
                RefundNumber = "QA-H2B-full-refund",
                Payment = fullyRefunded,
                RecordedByUser = user,
                Method = PaymentMethod.Tarjeta,
                Amount = 100m,
                Reason = marker,
                ExternalReference = "QA-full-refund"
            });
            context.CardSettlements.Add(new CardSettlement
            {
                SettlementNumber = "QA-void-settlement",
                ExternalReference = "QA-void-batch",
                RecordedByUser = user,
                SettlementDate = new DateOnly(2036, 1, 1),
                GrossAmount = 5m,
                BankDepositAmount = 5m,
                Status = CardSettlementStatus.Anulado,
                Applications = [new() { Payment = eligible[0], Amount = 5m }]
            });
            context.Payments.Add(NewPayment("QA-void-payment", PaymentStatus.Anulado));
            var transfer = NewPayment("QA-transfer-payment"); transfer.Method = PaymentMethod.Transferencia;
            context.Payments.Add(transfer);
            await context.SaveChangesAsync();
            // SaveChanges assigns CreatedAt once to an inserted batch; establish an explicit historical order afterwards.
            for (var index = 0; index < cashMovements.Count; index++)
                cashMovements[index].CreatedAt = new DateTime(2036, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(index);
            await context.SaveChangesAsync();
            registerId = registers[0].Id;
            partialPaymentId = eligible[0].Id;
            entryId = entries[0].Id;
            expectedDebit = await context.EntryItems.Where(item => !item.Entry.IsDeleted).SumAsync(item => item.Debit);
            expectedCredit = await context.EntryItems.Where(item => !item.Entry.IsDeleted).SumAsync(item => item.Credit);
        }
        async Task<JsonElement> Read(string path)
            => await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get, path, null, HttpStatusCode.OK);
        var metrics = new List<object>();
        foreach (var (path, legacyPath) in new[]
        {
            ($"/api/cash-registers/paged?search={marker}", "/api/cash-registers"),
            ($"/api/cash-registers/{registerId}/movements/paged?search={marker}", $"/api/cash-registers/{registerId}/movements"),
            ($"/api/card-settlements/paged?search={marker}", "/api/card-settlements"),
            ($"/api/card-settlements/eligible-payments/paged?search={marker}", "/api/card-settlements/eligible-payments"),
            ($"/api/accounts/journal-entries/paged?search={marker}", "/api/accounts/journal-entries"),
            ($"/api/backup/logs/paged?search={marker}", "/api/backup/logs?limit=100")
        })
        {
            var first = await Read(path + "&page=1&pageSize=10");
            var second = await Read(path + "&page=2&pageSize=10");
            var repeated = await Read(path + "&page=1&pageSize=10");
            var last = await Read(path + "&page=99&pageSize=10");
            Assert.Equal(31, first.GetProperty("totalItems").GetInt32());
            Assert.Equal(4, last.GetProperty("page").GetInt32());
            Assert.Single(last.GetProperty("items").EnumerateArray());
            var ids = first.GetProperty("items").EnumerateArray().Select(item => item.GetProperty("id").GetGuid()).ToArray();
            Assert.Equal(ids, repeated.GetProperty("items").EnumerateArray().Select(item => item.GetProperty("id").GetGuid()));
            Assert.DoesNotContain(second.GetProperty("items").EnumerateArray(), item => ids.Contains(item.GetProperty("id").GetGuid()));
            var legacy = await Read(legacyPath);
            metrics.Add(new
            {
                path,
                matchedRows = 31,
                legacyRows = legacy.GetArrayLength(),
                pageRows = first.GetProperty("items").GetArrayLength(),
                legacyBytes = Encoding.UTF8.GetByteCount(legacy.GetRawText()),
                pageBytes = Encoding.UTF8.GetByteCount(first.GetRawText())
            });
            using var anonymous = await client.GetAsync(path);
            Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);
        }
        var detail = await Read($"/api/cash-registers/{registerId}");
        Assert.Equal(130m, detail.GetProperty("currentBalance").GetDecimal());
        Assert.True(detail.GetProperty("isOpen").GetBoolean());
        var lastMovements = await Read($"/api/cash-registers/{registerId}/movements/paged?page=4&pageSize=10");
        Assert.Equal(100m, lastMovements.GetProperty("items")[0].GetProperty("balanceAfter").GetDecimal());
        Assert.Equal(130m, (await Read($"/api/cash-registers/{registerId}")).GetProperty("currentBalance").GetDecimal());
        var filteredMovements = await Read($"/api/cash-registers/{registerId}/movements/paged?movementType=Apertura");
        Assert.Equal(1, filteredMovements.GetProperty("totalItems").GetInt32());
        var legacyEligible = await Read("/api/card-settlements/eligible-payments");
        var available = legacyEligible.EnumerateArray().ToDictionary(item => item.GetProperty("id").GetGuid(), item => item.GetProperty("availableAmount").GetDecimal());
        Assert.Equal(70m, available[partialPaymentId]);
        var allEligible = await Read("/api/card-settlements/eligible-payments/paged?pageSize=100");
        Assert.Equal(available.Count, allEligible.GetProperty("totalItems").GetInt32());
        Assert.All(allEligible.GetProperty("items").EnumerateArray(), item =>
            Assert.Equal(available[item.GetProperty("id").GetGuid()], item.GetProperty("availableAmount").GetDecimal()));
        var eligibleSummary = await Read("/api/card-settlements/eligible-payments/summary");
        Assert.Equal(available.Count, eligibleSummary.GetProperty("totalPayments").GetInt32());
        Assert.Equal(available.Values.Sum(), eligibleSummary.GetProperty("availableAmount").GetDecimal());
        var journalSummary = await Read("/api/accounts/journal-entries/summary");
        Assert.Equal(expectedDebit, journalSummary.GetProperty("debit").GetDecimal());
        Assert.Equal(expectedCredit, journalSummary.GetProperty("credit").GetDecimal());
        var entryList = await Read($"/api/accounts/journal-entries/paged?search={marker}&pageSize=1");
        Assert.False(entryList.GetProperty("items")[0].TryGetProperty("items", out _));
        Assert.Equal(2, entryList.GetProperty("items")[0].GetProperty("itemCount").GetInt32());
        var entryDetail = await Read($"/api/accounts/journal-entries/{entryId}");
        Assert.Equal(2, entryDetail.GetProperty("items").GetArrayLength());
        var accountSearch = await Read("/api/accounts/journal-entries/paged?search=DiAr");
        Assert.True(accountSearch.GetProperty("totalItems").GetInt32() >= 31);
        var backups = await Read($"/api/backup/logs/paged?search={marker}&status=CreatedLocal");
        Assert.Equal(16, backups.GetProperty("totalItems").GetInt32());
        Assert.All(backups.GetProperty("items").EnumerateArray(), item =>
        {
            Assert.False(item.TryGetProperty("localPath", out _));
            Assert.False(item.TryGetProperty("googleDriveFileId", out _));
        });
        foreach (var (path, status) in new[]
        {
            ($"/api/cash-registers/{Guid.NewGuid()}/movements/paged", HttpStatusCode.NotFound),
            ($"/api/cash-registers/{registerId}/movements/paged?movementType=99", HttpStatusCode.BadRequest),
            ("/api/backup/logs?limit=0", HttpStatusCode.BadRequest),
            ("/api/backup/logs/paged?status=unknown", HttpStatusCode.BadRequest),
            ("/api/card-settlements/eligible-payments/paged?pageSize=101", HttpStatusCode.BadRequest)
        })
        {
            using var request = Authorized(HttpMethod.Get, path, accessToken);
            using var response = await client.SendAsync(request);
            await AssertSafeProblemDetailsAsync(response, status);
        }
        var empty = await Read("/api/accounts/journal-entries/paged?search=not-existing-H2B");
        Assert.Equal(0, empty.GetProperty("totalItems").GetInt32());
        using var schemaResponse = await client.GetAsync("/openapi/v1.json");
        using var schema = JsonDocument.Parse(await schemaResponse.Content.ReadAsStringAsync());
        var cashOpen = schema.RootElement.GetProperty("paths").GetProperty("/api/cash-registers/{id}/open").GetProperty("post");
        Assert.EndsWith("/CashRegisterOperationDto", cashOpen.GetProperty("responses").GetProperty("200").GetProperty("content")
            .GetProperty("application/json").GetProperty("schema").GetProperty("$ref").GetString());
        var metricsPath = Environment.GetEnvironmentVariable("HOTEL_ERP_FINANCE_PAGINATION_METRICS");
        if (!string.IsNullOrWhiteSpace(metricsPath)) await File.WriteAllTextAsync(metricsPath,
            JsonSerializer.Serialize(new
            {
                syntheticDatabase = true,
                lists = metrics,
                partialPaymentAvailable = 70m,
                cashBalance = 130m,
                journalDebit = expectedDebit,
                journalCredit = expectedCredit
            }, new JsonSerializerOptions { WriteIndented = true }));
    }
}
