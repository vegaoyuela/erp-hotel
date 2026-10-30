using System.Net;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    [PostgreSqlFact]
    public async Task CatalogMutations_OrderRevocationLogoutCancellationAndConcurrentProfileApproval()
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
            await GrantCatalogPermissionsAsync(factory.Services, ids);
            var cases = await PrepareCatalogCasesAsync(factory.Services);
            var admin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
            var worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
            Task<HttpResponseMessage> Revoke() => SecuritySendAsync(client, admin.AccessToken, HttpMethod.Put,
                $"/api/roles/{ids.WorkerRole}", new { permissions = Array.Empty<string>() });
            await using (var blocker = new NpgsqlConnection(database.ConnectionString))
            {
                await blocker.OpenAsync();
                await using var tx = await blocker.BeginTransactionAsync();
                await using (var command = blocker.CreateCommand())
                {
                    command.Transaction = tx;
                    command.CommandText = "SELECT pg_advisory_xact_lock(hashtext('hotel-catalog:Settings'))";
                    await command.ExecuteNonQueryAsync();
                }
                var operation = SendCatalogCaseAsync(client, worker.AccessToken, cases.Single(item => item.Name == "Settings.Update"));
                await WaitForOperationalWaitersAsync(database.ConnectionString, 1);
                var revoke = Revoke();
                try { await WaitForOperationalWaitersAsync(database.ConnectionString, 2); }
                finally { await tx.RollbackAsync(); }
                using (var response = await operation) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                using (var response = await revoke) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
            await AssertEndpointStatusAsync(client, worker.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);
            using (var scope = factory.Services.CreateScope())
                Assert.Equal(CatalogPrivateMarker, (await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().BusinessSettings.SingleAsync()).Footer);

            await GrantCatalogPermissionsAsync(factory.Services, ids);
            worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
            var attachment = cases.Single(item => item.WithFile);
            injector.HoldNext(ApplicationFailureCheckpoints.CatalogAfterAttachment);
            var upload = SendCatalogCaseAsync(client, worker.AccessToken, attachment);
            await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
            var logout = SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post, "/api/auth/logout");
            try { await WaitForOperationalWaitersAsync(database.ConnectionString, 1); }
            finally { injector.Release(); }
            using (var response = await upload) Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            using (var response = await logout) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(Directory.GetFiles(files));
            await AssertEndpointStatusAsync(client, worker.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);

            worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
            var body = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(JsonSerializer.Serialize(attachment.Body))!;
            body["caiNumber"] = JsonSerializer.SerializeToElement("QA-CANCELED-AUTHORIZATION");
            var canceledCase = attachment with { Body = body };
            var before = await CatalogStateDigestAsync(database.ConnectionString, false);
            var beforeFiles = CatalogFilesDigest(files);
            injector.HoldNext(ApplicationFailureCheckpoints.CatalogAfterAttachment);
            using var cancellation = new CancellationTokenSource();
            var canceled = SendCatalogCaseAsync(client, worker.AccessToken, canceledCase, cancellation.Token);
            await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
            var waitingRevoke = Revoke();
            try
            {
                await WaitForOperationalWaitersAsync(database.ConnectionString, 1);
                cancellation.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => canceled);
            }
            finally { injector.Release(); }
            using (var response = await waitingRevoke) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(before, await CatalogStateDigestAsync(database.ConnectionString, false));
            Assert.Equal(beforeFiles, CatalogFilesDigest(files));

            var approveCase = cases.Single(item => item.Name == "Settings.ApproveFiscalProfile");
            injector.HoldNext(ApplicationFailureCheckpoints.CatalogAfterPersistence);
            var approveFirst = SendCatalogCaseAsync(client, admin.AccessToken, approveCase);
            await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
            var approveSecond = SendCatalogCaseAsync(client, admin.AccessToken, approveCase);
            try { await WaitForOperationalWaitersAsync(database.ConnectionString, 1); }
            finally { injector.Release(); }
            using (var response = await approveFirst) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using (var response = await approveSecond) await AssertSafeProblemDetailsAsync(response, HttpStatusCode.Conflict, "Versión fiscal inmutable");
            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                Assert.Equal(1, await db.FiscalProfileRevisions.CountAsync());
                Assert.Equal(1, await db.AuditLogs.CountAsync(log => log.Action == "ApproveFiscalProfile"));
                Assert.True((await scope.ServiceProvider.GetRequiredService<AuditChainService>().VerifyAsync()).IsValid);
            }
            await WriteSecurityMetricsAsync("p02l-orden.json", new
            {
                syntheticDatabase = true,
                passed = true,
                settingsMutationBeforeRevocation = true,
                uploadedAuthorizationBeforeOwnLogout = true,
                canceledUploadRemovesUncommittedFileAndReleasesLocks = true,
                successfulFilesPreserved = 1,
                concurrentProfileApprovals = new[] { 200, 409 },
                immutableProfileSnapshots = 1,
                approvalAuditEvents = 1,
                auditChainValid = true
            });
        }
        finally { Directory.Delete(files, true); }
    }
}
