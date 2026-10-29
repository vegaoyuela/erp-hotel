using System.Net;
using System.Net.Http.Json;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    [PostgreSqlFact]
    public async Task SecurityAdministration_SerializesRacesAndRevalidatesWaitingSessions()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        await using var database = new TemporaryPostgreSqlDatabase(Environment.GetEnvironmentVariable("HOTEL_ERP_TEST_ADMIN_CONNECTION")!);
        await database.CreateAsync();
        using var environment = new EnvironmentVariableScope(SecurityTestEnvironment(database.ConnectionString));
        var injector = new SecurityTestInjector();
        await using var factory = SecurityFactory(database.ConnectionString, injector);
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var ids = await SeedSecurityActorsAsync(factory.Services);
        async Task RestoreTwoAdmins()
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            foreach (var id in new[] { ids.Admin, ids.SecondAdmin })
            {
                var user = await db.Users.SingleAsync(row => row.Id == id);
                user.IsActive = true;
                user.SecurityVersion++;
                if (!await db.UserRoles.AnyAsync(link => link.UserId == id && link.RoleId == ids.AdminRole))
                    db.UserRoles.Add(new UserRole { UserId = id, RoleId = ids.AdminRole });
            }
            await db.SaveChangesAsync();
        }
        async Task AssertOneAdmin()
        {
            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(1, await db.Users.CountAsync(row => row.IsActive && !row.IsDeleted
                && row.UserRoles.Any(link => !link.Role.IsDeleted && link.Role.SystemKey == SystemRoleKeys.Administrator)));
        }
        await RestoreTwoAdmins();
        var firstAdmin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
        var secondAdmin = await LoginAsync(client, SecurityTestPassword, "security_second_admin");
        injector.HoldNextAdministration();
        var firstDemotion = SecuritySendAsync(client, firstAdmin.AccessToken, HttpMethod.Put, $"/api/users/{ids.Admin}", new { roles = Array.Empty<string>() });
        await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var secondDemotion = SecuritySendAsync(client, secondAdmin.AccessToken, HttpMethod.Put, $"/api/users/{ids.SecondAdmin}", new { roles = Array.Empty<string>() });
        try { await WaitForSecurityLockWaiterAsync(database.ConnectionString); }
        finally { injector.Release(); }
        using (var first = await firstDemotion) Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        using (var second = await secondDemotion) Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        await AssertOneAdmin();

        await RestoreTwoAdmins();
        firstAdmin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
        secondAdmin = await LoginAsync(client, SecurityTestPassword, "security_second_admin");
        injector.HoldNextAdministration();
        var firstDeactivation = SecuritySendAsync(client, firstAdmin.AccessToken, HttpMethod.Delete, $"/api/users/{ids.SecondAdmin}");
        await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var secondDeactivation = SecuritySendAsync(client, secondAdmin.AccessToken, HttpMethod.Delete, $"/api/users/{ids.Admin}");
        try { await WaitForSecurityLockWaiterAsync(database.ConnectionString); }
        finally { injector.Release(); }
        using (var first = await firstDeactivation) Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        using (var second = await secondDeactivation) Assert.Equal(HttpStatusCode.Unauthorized, second.StatusCode);
        await AssertOneAdmin();

        var admin = firstAdmin;
        var delegated = await LoginAsync(client, SecurityTestPassword, "security_delegate");
        injector.HoldNextAdministration();
        var revokePermission = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Put, $"/api/roles/{ids.DelegateRole}", new { permissions = Array.Empty<string>() });
        await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var waitingWrite = SecuritySendAsync(client, delegated.AccessToken, HttpMethod.Post, "/api/roles", new { name = "Must not be created after revocation", permissions = Array.Empty<string>() });
        try { await WaitForSecurityLockWaiterAsync(database.ConnectionString); }
        finally { injector.Release(); }
        using (var response = await revokePermission) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        using (var response = await waitingWrite) Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        await AssertEndpointStatusAsync(client, delegated.AccessToken, "/api/roles", HttpStatusCode.Unauthorized);
        using (var response = await client.PostAsJsonAsync("/api/auth/refresh", new { refreshToken = delegated.RefreshToken }))
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        using (var scope = factory.Services.CreateScope())
            Assert.False(await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Roles.AnyAsync(row => row.Name == "Must not be created after revocation"));

        injector.HoldNextAdministration();
        var assignment = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Post, $"/api/users/{ids.Worker}/roles/{ids.ExtraRole}");
        await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var deleteAssignedRole = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Delete, $"/api/roles/{ids.ExtraRole}");
        try { await WaitForSecurityLockWaiterAsync(database.ConnectionString); }
        finally { injector.Release(); }
        using (var response = await assignment) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using (var response = await deleteAssignedRole) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        Assert.DoesNotContain(PermissionNames.ManageInventory, worker.User!.Permissions);

        injector.HoldNextAdministration();
        const string newPassword = "Concurrent!123456";
        var resetPassword = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Post, $"/api/users/{ids.Worker}/reset-password", new { newPassword });
        await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var waitingLogin = client.PostAsJsonAsync("/api/auth/login", new { username = "security_worker", password = SecurityTestPassword });
        try { await WaitForSecurityLockWaiterAsync(database.ConnectionString); }
        finally { injector.Release(); }
        using (var response = await resetPassword) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using (var response = await waitingLogin) Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        worker = await LoginAsync(client, newPassword, "security_worker");

        injector.HoldNextAdministration();
        const string newerPassword = "Concurrent!987654";
        var secondReset = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Post, $"/api/users/{ids.Worker}/reset-password", new { newPassword = newerPassword });
        await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var waitingRefresh = client.PostAsJsonAsync("/api/auth/refresh", new { refreshToken = worker.RefreshToken });
        try { await WaitForSecurityLockWaiterAsync(database.ConnectionString); }
        finally { injector.Release(); }
        using (var response = await secondReset) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using (var response = await waitingRefresh) Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await db.Users.SingleAsync(row => row.Id == ids.Worker);
            Assert.True(BCrypt.Net.BCrypt.Verify(newerPassword, user.PasswordHash));
            Assert.Equal(0, await db.RefreshTokens.CountAsync(row => row.UserId == ids.Worker && !row.IsRevoked));
        }

        worker = await LoginAsync(client, newerPassword, "security_worker");
        var refreshes = await Task.WhenAll(Enumerable.Range(0, 2).Select(_ => client.PostAsJsonAsync("/api/auth/refresh", new { refreshToken = worker.RefreshToken })));
        Assert.Equal(new[] { HttpStatusCode.OK, HttpStatusCode.Unauthorized }, refreshes.Select(response => response.StatusCode).Order().ToArray());
        var rotated = (await refreshes.Single(response => response.StatusCode == HttpStatusCode.OK).Content.ReadFromJsonAsync<AuthResponse>())!;
        await AssertEndpointStatusAsync(client, rotated.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);
        foreach (var response in refreshes) response.Dispose();

        var lockoutAttempts = await Task.WhenAll(Enumerable.Range(0, 5).Select(_ => client.PostAsJsonAsync("/api/auth/login", new { username = "security_worker", password = "Invalid!123456" })));
        Assert.All(lockoutAttempts, response => Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode));
        foreach (var response in lockoutAttempts) response.Dispose();
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await db.Users.SingleAsync(row => row.Id == ids.Worker);
            Assert.Equal(5, user.FailedLoginAttempts);
            Assert.NotNull(user.LockoutEnd);
            Assert.True((await scope.ServiceProvider.GetRequiredService<hotel_erp.Api.Services.AuditChainService>().VerifyAsync()).IsValid);
        }
        await WriteSecurityMetricsAsync("p02j-concurrencia.json", new
        {
            syntheticDatabase = true,
            passed = true,
            races = new[] { "two-self-demotions", "cross-deactivation", "waiting-privilege-revocation", "assign-then-delete-role", "password-reset-vs-login", "password-reset-vs-refresh", "concurrent-refresh-replay", "concurrent-failed-login" },
            waitingRequestsObservedInPostgreSql = 6,
            lastAdministratorPreserved = true,
            staleSessionCannotWrite = true,
            replayRevokesReplacementSession = true,
            failedLoginAttemptsPreserved = 5,
            auditChainValid = true
        });
    }
}
