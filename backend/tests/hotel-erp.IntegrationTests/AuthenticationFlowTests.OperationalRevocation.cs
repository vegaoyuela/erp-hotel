using System.Net;
using System.Reflection;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    [PostgreSqlFact]
    public async Task OperationalMutations_RevalidateAllMethodsAfterWaitingForRevocation()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        await using var database = new TemporaryPostgreSqlDatabase(Environment.GetEnvironmentVariable("HOTEL_ERP_TEST_ADMIN_CONNECTION")!);
        await database.CreateAsync();
        using var environment = new EnvironmentVariableScope(SecurityTestEnvironment(database.ConnectionString));
        var injector = new OperationalRaceInjector();
        await using var factory = OperationalFactory(database.ConnectionString, injector);
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var ids = await SeedSecurityActorsAsync(factory.Services);
        await PrepareOperationalWorkerAsync(factory.Services, ids);
        var admin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
        var worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        var actor = await OperationalActorAsync(factory.Services, ids.Worker);
        var methods = OperationalServices.SelectMany(service => service.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(method => typeof(Task).IsAssignableFrom(method.ReturnType)).Select(method => (Service: service, Method: method))).ToArray();
        Assert.Equal(27, methods.Length);
        var before = await OperationalBusinessDigestAsync(database.ConnectionString);
        injector.Administration.HoldNextAdministration();
        var revoke = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Put, $"/api/roles/{ids.WorkerRole}", new { permissions = Array.Empty<string>() });
        await injector.Administration.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var direct = methods.Select(item => AssertDirectOperationalDeniedAsync(factory.Services, item.Service, item.Method, actor, 401)).ToArray();
        var http = new[]
        {
            SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post, "/api/inventory/categories", new { name = "Must not persist QA" }),
            SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post, "/api/cash-registers", new { name = "Must not persist QA" }),
            SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post, $"/api/reservations/{Guid.NewGuid()}/confirm", new { expectedVersion = 1 })
        };
        try { await WaitForOperationalWaitersAsync(database.ConnectionString, 30); }
        finally { injector.Administration.Release(); }
        using (var response = await revoke) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        await Task.WhenAll(direct);
        foreach (var task in http)
        {
            using var response = await task;
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.Unauthorized);
            Assert.Equal("no-store", response.Headers.CacheControl?.ToString());
        }
        Assert.Equal(before, await OperationalBusinessDigestAsync(database.ConnectionString));
        var currentActor = await OperationalActorAsync(factory.Services, ids.Worker);
        await Task.WhenAll(methods.Select(item => AssertDirectOperationalDeniedAsync(factory.Services, item.Service, item.Method, currentActor, 403)));
        Assert.Equal(before, await OperationalBusinessDigestAsync(database.ConnectionString));

        // A deliberately inconsistent fixture (permission removed without a version bump) proves live authorization too.
        await PrepareOperationalWorkerAsync(factory.Services, ids);
        worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.RolePermissions.RemoveRange(await db.RolePermissions.Where(link => link.RoleId == ids.WorkerRole
                && link.Permission.Name == PermissionNames.ManageInventory).ToListAsync());
            await db.SaveChangesAsync();
        }
        using (var response = await SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post,
            "/api/inventory/categories?secret=must-not-be-audited", new { name = "Must not persist QA" }))
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.Forbidden);
        Assert.Equal(before, await OperationalBusinessDigestAsync(database.ConnectionString));
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var log = await db.AuditLogs.SingleAsync(row => row.Action == "OperationalAccessDenied");
            Assert.DoesNotContain("must-not-be-audited", log.Changes ?? "");
            Assert.DoesNotContain("secret=", log.Changes ?? "");
        }
        await WriteSecurityMetricsAsync("p02k-revocacion.json", new
        {
            syntheticDatabase = true,
            passed = true,
            directMethods = methods.Select(item => item.Service.Name + "." + item.Method.Name),
            staleSessionRejections = 27,
            livePermissionRejections = 27,
            queuedHttpRejections = 3,
            postgresWaitersObserved = 30,
            liveHttpDenial = 403,
            businessStateUnchanged = true,
            includesFiscalCorrelativesPaymentsAndInventory = true,
            deniedAuditExcludesRequestSecrets = true
        });
    }
}
