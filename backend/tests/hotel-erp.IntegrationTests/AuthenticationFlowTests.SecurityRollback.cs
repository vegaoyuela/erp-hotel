using System.Net;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    [PostgreSqlFact]
    public async Task SecurityAdministration_RollsBackChangesTokensAndAuditAtEveryCheckpoint()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        await using var database = new TemporaryPostgreSqlDatabase(Environment.GetEnvironmentVariable("HOTEL_ERP_TEST_ADMIN_CONNECTION")!);
        await database.CreateAsync();
        using var environment = new EnvironmentVariableScope(SecurityTestEnvironment(database.ConnectionString));
        var injector = new SecurityTestInjector();
        await using var factory = SecurityFactory(database.ConnectionString, injector);
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var ids = await SeedSecurityActorsAsync(factory.Services);
        var admin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
        var worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        var cases = new (string Name, string? Token, HttpMethod Method, string Path, object? Body)[]
        {
            ("create-user", admin.AccessToken, HttpMethod.Post, "/api/users", new { username = "rollback_user", password = "Rollback!123456", email = "rollback@example.invalid", firstName = "Rollback", lastName = "QA", roles = new[] { "Recepcion" } }),
            ("update-user", admin.AccessToken, HttpMethod.Put, $"/api/users/{ids.Worker}", new { firstName = "MustRollback", roles = new[] { "Security QA Worker", "Security QA Extra" } }),
            ("reset-password", admin.AccessToken, HttpMethod.Post, $"/api/users/{ids.Worker}/reset-password", new { newPassword = "Rollback!123456" }),
            ("deactivate-user", admin.AccessToken, HttpMethod.Delete, $"/api/users/{ids.Worker}", null),
            ("assign-role", admin.AccessToken, HttpMethod.Post, $"/api/users/{ids.Worker}/roles/{ids.ExtraRole}", null),
            ("create-role", admin.AccessToken, HttpMethod.Post, "/api/roles", new { name = "Rollback role", permissions = new[] { "view_reports" } }),
            ("update-role", admin.AccessToken, HttpMethod.Put, $"/api/roles/{ids.WorkerRole}", new { name = "Must rollback role", permissions = new[] { "manage_inventory" } }),
            ("delete-role", admin.AccessToken, HttpMethod.Delete, $"/api/roles/{ids.WorkerRole}", null),
            ("legacy-register", admin.AccessToken, HttpMethod.Post, "/api/auth/register", new { username = "rollback_registration", password = "Rollback!123456", email = "rollback-registration@example.invalid", firstName = "Rollback", lastName = "QA" }),
            ("change-password", worker.AccessToken, HttpMethod.Post, "/api/auth/change-password", new { currentPassword = SecurityTestPassword, newPassword = "Rollback!123456" }),
            ("logout", worker.AccessToken, HttpMethod.Post, "/api/auth/logout", null),
            ("login", null, HttpMethod.Post, "/api/auth/login", new { username = "security_worker", password = SecurityTestPassword }),
            ("refresh", null, HttpMethod.Post, "/api/auth/refresh", new { refreshToken = worker.RefreshToken })
        };
        var results = new List<object>();
        foreach (var checkpoint in new[] { ApplicationFailureCheckpoints.SecurityAfterPersistence,
            ApplicationFailureCheckpoints.SecurityAfterRevocation, ApplicationFailureCheckpoints.SecurityAfterAudit })
            foreach (var item in cases)
            {
                var before = await SecurityStateDigestAsync(database.ConnectionString);
                injector.FailAt = checkpoint;
                using var response = await SecuritySendAsync(client, item.Token, item.Method, item.Path, item.Body);
                injector.FailAt = null;
                await AssertSafeProblemDetailsAsync(response, HttpStatusCode.InternalServerError);
                Assert.Equal(before, await SecurityStateDigestAsync(database.ConnectionString));
                results.Add(new { operation = item.Name, checkpoint, status = 500, fullStateRestored = true });
            }
        await WriteSecurityMetricsAsync("p02j-rollback.json", new { syntheticDatabase = true, passed = true, scenarios = results });
    }
}
