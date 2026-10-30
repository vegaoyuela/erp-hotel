using System.Net;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    [PostgreSqlFact]
    public async Task CatalogMutations_All34ActionsPersistAuditAndRollbackAtEveryCheckpoint()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        await using var database = new TemporaryPostgreSqlDatabase(Environment.GetEnvironmentVariable("HOTEL_ERP_TEST_ADMIN_CONNECTION")!);
        await database.CreateAsync();
        using var environment = new EnvironmentVariableScope(SecurityTestEnvironment(database.ConnectionString));
        var files = Path.Combine(Path.GetTempPath(), "hotel-catalog-qa-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(files);
        try
        {
            var injector = new CatalogTestInjector();
            await using var factory = CatalogFactory(database.ConnectionString, injector, files);
            using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var ids = await SeedSecurityActorsAsync(factory.Services);
            var cases = await PrepareCatalogCasesAsync(factory.Services);
            Assert.Equal(35, cases.Count); // 34 actions plus the deactivate-with-history branch.
            var admin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
            var rollbacks = new List<object>();
            var successes = new List<object>();
            foreach (var item in cases)
            {
                var checkpoints = new List<string> { ApplicationFailureCheckpoints.CatalogAfterPersistence, ApplicationFailureCheckpoints.CatalogAfterAudit };
                if (item.WithFile) checkpoints.Add(ApplicationFailureCheckpoints.CatalogAfterAttachment);
                foreach (var checkpoint in checkpoints)
                {
                    var before = await CatalogStateDigestAsync(database.ConnectionString, true);
                    var beforeFiles = CatalogFilesDigest(files);
                    injector.Security.FailAt = checkpoint;
                    using var response = await SendCatalogCaseAsync(client, admin.AccessToken, item);
                    injector.Security.FailAt = null;
                    await AssertSafeProblemDetailsAsync(response, HttpStatusCode.InternalServerError);
                    Assert.Equal(before, await CatalogStateDigestAsync(database.ConnectionString, true));
                    Assert.Equal(beforeFiles, CatalogFilesDigest(files));
                    rollbacks.Add(new { operation = item.Name, checkpoint, databaseRestored = true, filesRestored = true });
                }
                List<Guid> auditIds;
                using (var scope = factory.Services.CreateScope()) auditIds = await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().AuditLogs.Select(log => log.Id).ToListAsync();
                using var success = await SendCatalogCaseAsync(client, admin.AccessToken, item);
                Assert.True(success.StatusCode == item.Success, $"{item.Name}: expected {(int)item.Success}, received {(int)success.StatusCode}: {await success.Content.ReadAsStringAsync()}");
                var expectedTarget = item.Path.Split('/').Select(segment => Guid.TryParse(segment, out var id) ? id : Guid.Empty)
                    .FirstOrDefault(id => id != Guid.Empty);
                if (item.Name.StartsWith("Settings.", StringComparison.Ordinal))
                {
                    using var settingsScope = factory.Services.CreateScope();
                    expectedTarget = await settingsScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().BusinessSettings.Select(row => row.Id).SingleAsync();
                }
                if (item.Success == HttpStatusCode.Created)
                {
                    Assert.NotNull(success.Headers.Location);
                    using var json = JsonDocument.Parse(await success.Content.ReadAsStringAsync());
                    expectedTarget = json.RootElement.GetProperty("id").GetGuid();
                    Assert.NotEqual(Guid.Empty, expectedTarget);
                    if (item.WithFile) Assert.True(json.RootElement.GetProperty("hasAttachment").GetBoolean());
                    if (item.Name == "Accounting.CreateJournalEntry")
                    {
                        Assert.Equal(5m, json.RootElement.GetProperty("totalDebit").GetDecimal());
                        Assert.Equal(5m, json.RootElement.GetProperty("totalCredit").GetDecimal());
                        Assert.Equal(2, json.RootElement.GetProperty("items").GetArrayLength());
                    }
                }
                using (var scope = factory.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    Assert.Equal(auditIds.Count + 1, await db.AuditLogs.CountAsync());
                    var log = await db.AuditLogs.SingleAsync(log => !auditIds.Contains(log.Id));
                    Assert.NotEqual(Guid.Empty, expectedTarget);
                    Assert.Equal(expectedTarget, log.EntityId);
                    Assert.Equal(ids.Admin, log.UserId);
                    Assert.DoesNotContain(CatalogPrivateMarker, log.Changes ?? "");
                    Assert.DoesNotContain("catalog-private.pdf", log.Changes ?? "");
                    if (item.Name == "Accounting.DeactivateWithHistory")
                    {
                        var target = Guid.Parse(item.Path.Split('/').Last());
                        var account = await db.AccountingAccounts.SingleAsync(row => row.Id == target);
                        Assert.False(account.IsActive);
                        Assert.False(account.IsDeleted);
                        Assert.True(await db.EntryItems.AnyAsync(row => row.AccountId == target));
                    }
                }
                successes.Add(new { operation = item.Name, status = (int)item.Success, auditCommittedTogether = true });
            }
            using (var scope = factory.Services.CreateScope())
            {
                var committer = scope.ServiceProvider.GetRequiredService<CatalogMutationCommitter>();
                await Assert.ThrowsAsync<InvalidOperationException>(() => committer.LockAsync("must-reject", CancellationToken.None));
                await Assert.ThrowsAsync<InvalidOperationException>(() => committer.PersistAsync(new(ids.Admin, 1), "must-reject", "Endpoint", () => ids.Admin, CancellationToken.None));
                Assert.True((await scope.ServiceProvider.GetRequiredService<AuditChainService>().VerifyAsync()).IsValid);
            }
            Assert.Single(Directory.GetFiles(files));
            Assert.Equal(71, rollbacks.Count);
            await WriteSecurityMetricsAsync("p02l-atomicidad.json", new
            {
                syntheticDatabase = true,
                passed = true,
                distinctHttpActions = 34,
                successfulCases = successes,
                rollbackCases = rollbacks,
                successfulAttachments = 1,
                uncoordinatedCommitterCallsRejected = 2,
                privateValuesExcludedFromAudit = true,
                historicalAccountPreserved = true,
                auditChainValid = true
            });
        }
        finally { Directory.Delete(files, true); }
    }
}
