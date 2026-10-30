using System.Net;
using System.Reflection;
using hotel_erp.Api.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    [PostgreSqlFact]
    public async Task CatalogMutations_RevalidateAll34MethodsAndHttpActionsAfterRevocation()
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
            var cases = (await PrepareCatalogCasesAsync(factory.Services)).Take(34).ToArray();
            var admin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
            var worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
            var actor = await OperationalActorAsync(factory.Services, ids.Worker);
            var methods = CatalogServices.SelectMany(service => service.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(method => typeof(Task).IsAssignableFrom(method.ReturnType)).Select(method => (Service: service, Method: method))).ToArray();
            Assert.Equal(34, methods.Length);
            var before = await CatalogStateDigestAsync(database.ConnectionString, false);
            var beforeFiles = CatalogFilesDigest(files);
            injector.Security.HoldNextAdministration();
            var revoke = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Put, $"/api/roles/{ids.WorkerRole}", new { permissions = Array.Empty<string>() });
            await injector.Security.Entered.WaitAsync(TimeSpan.FromSeconds(10));
            var direct = methods.Select(item => AssertDirectOperationalDeniedAsync(factory.Services, item.Service, item.Method, actor, 401)).ToArray();
            var http = cases.Select(item => SendCatalogCaseAsync(client, worker.AccessToken, item)).ToArray();
            try { await WaitForOperationalWaitersAsync(database.ConnectionString, 68); }
            finally { injector.Security.Release(); }
            using (var response = await revoke) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            await Task.WhenAll(direct);
            foreach (var task in http)
            {
                using var response = await task;
                await AssertSafeProblemDetailsAsync(response, HttpStatusCode.Unauthorized);
                Assert.Equal("no-store", response.Headers.CacheControl?.ToString());
            }
            Assert.Equal(before, await CatalogStateDigestAsync(database.ConnectionString, false));
            Assert.Equal(beforeFiles, CatalogFilesDigest(files));

            // Fixture intentionally removes permissions without a version bump, proving live permission lookup too.
            await GrantCatalogPermissionsAsync(factory.Services, ids);
            worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
            actor = await OperationalActorAsync(factory.Services, ids.Worker);
            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.RolePermissions.RemoveRange(await db.RolePermissions.Where(link => link.RoleId == ids.WorkerRole).ToListAsync());
                await db.SaveChangesAsync();
            }
            await Task.WhenAll(methods.Select(item => AssertDirectOperationalDeniedAsync(factory.Services, item.Service, item.Method, actor, 403)));
            foreach (var item in cases)
            {
                using var response = await SendCatalogCaseAsync(client, worker.AccessToken, item with { Path = item.Path + "?private=" + CatalogPrivateMarker });
                await AssertSafeProblemDetailsAsync(response, HttpStatusCode.Forbidden);
                Assert.Equal("no-store", response.Headers.CacheControl?.ToString());
            }
            Assert.Equal(before, await CatalogStateDigestAsync(database.ConnectionString, false));
            Assert.Equal(beforeFiles, CatalogFilesDigest(files));
            using (var scope = factory.Services.CreateScope())
            {
                var logs = await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().AuditLogs.Where(log => log.Action == "OperationalAccessDenied").ToListAsync();
                Assert.Equal(34, logs.Count);
                Assert.All(logs, log => Assert.DoesNotContain(CatalogPrivateMarker, log.Changes ?? ""));
            }
            await WriteSecurityMetricsAsync("p02l-revocacion.json", new
            {
                syntheticDatabase = true,
                passed = true,
                serviceMethods = methods.Select(item => item.Service.Name + "." + item.Method.Name),
                directStaleSessionDenials = 34,
                directLivePermissionDenials = 34,
                queuedHttpStaleSessionDenials = 34,
                liveHttpPermissionDenials = 34,
                postgresWaitersObserved = 68,
                businessStateUnchanged = true,
                filesUnchanged = true,
                auditedLiveHttpDenials = 34,
                requestSecretsExcludedFromAudit = true
            });
        }
        finally { Directory.Delete(files, true); }
    }
}
