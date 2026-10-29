using System.Net;
using System.Net.Http.Json;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    [PostgreSqlFact]
    public async Task SecurityAdministration_PreservesAdministratorAndAuditsSuccessfulChanges()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        await using var database = new TemporaryPostgreSqlDatabase(Environment.GetEnvironmentVariable("HOTEL_ERP_TEST_ADMIN_CONNECTION")!);
        await database.CreateAsync();
        using var environment = new EnvironmentVariableScope(SecurityTestEnvironment(database.ConnectionString));
        await using var factory = SecurityFactory(database.ConnectionString, new SecurityTestInjector());
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var ids = await SeedSecurityActorsAsync(factory.Services);
        var admin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
        var full = await LoginAsync(client, SecurityTestPassword, "security_full_delegate");
        var role = await SendPersistenceJsonAsync(client, admin.AccessToken!, HttpMethod.Post, "/api/roles",
            new { name = "Successful QA Role", permissions = new[] { PermissionNames.ViewReports } }, HttpStatusCode.Created);
        var roleId = role.GetProperty("id").GetGuid();
        var created = await SendPersistenceJsonAsync(client, admin.AccessToken!, HttpMethod.Post, "/api/users",
            new { username = "secure_new", password = SecurityTestPassword, email = "secure-new@example.invalid", firstName = "Usuario", lastName = "QA", roles = new[] { "Successful QA Role" } }, HttpStatusCode.Created);
        var userId = created.GetProperty("id").GetGuid();
        var session = await LoginAsync(client, SecurityTestPassword, "secure_new");
        var updated = await SendPersistenceJsonAsync(client, admin.AccessToken!, HttpMethod.Put, $"/api/users/{userId}",
            new { firstName = "Actualizado", roles = new[] { "Successful QA Role", "Successful QA Role" } }, HttpStatusCode.OK);
        Assert.Single(updated.GetProperty("roles").EnumerateArray());
        Assert.Equal("Actualizado", updated.GetProperty("firstName").GetString());
        await AssertEndpointStatusAsync(client, session.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);
        using (var refresh = await client.PostAsJsonAsync("/api/auth/refresh", new { refreshToken = session.RefreshToken }))
            Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);
        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Post, $"/api/users/{userId}/roles/{ids.ExtraRole}", null, HttpStatusCode.OK);
        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Post, $"/api/users/{userId}/roles/{ids.ExtraRole}", null, HttpStatusCode.BadRequest);
        const string replacementPassword = "Replacement!123456";
        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Post, $"/api/users/{userId}/reset-password", new { newPassword = replacementPassword }, HttpStatusCode.OK);
        var reset = await LoginAsync(client, replacementPassword, "secure_new");
        Assert.True(reset.User!.MustChangePassword);
        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Delete, $"/api/users/{userId}", null, HttpStatusCode.OK);
        await AssertEndpointStatusAsync(client, reset.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);
        using (var refresh = await client.PostAsJsonAsync("/api/auth/refresh", new { refreshToken = reset.RefreshToken }))
            Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);

        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Post, $"/api/users/{ids.Worker}/roles/{ids.ExtraRole}", null, HttpStatusCode.OK);
        var worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Put, $"/api/roles/{ids.ExtraRole}",
            new { permissions = new[] { PermissionNames.ManageInventory, PermissionNames.ViewReports } }, HttpStatusCode.NoContent);
        await AssertEndpointStatusAsync(client, worker.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);
        using (var refresh = await client.PostAsJsonAsync("/api/auth/refresh", new { refreshToken = worker.RefreshToken }))
            Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);
        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Delete, $"/api/roles/{ids.ExtraRole}", null, HttpStatusCode.NoContent);
        var afterRoleDeletion = await LoginAsync(client, SecurityTestPassword, "security_worker");
        Assert.DoesNotContain(PermissionNames.ManageInventory, afterRoleDeletion.User!.Permissions);
        await SendPersistenceStatusAsync(client, admin.AccessToken!, HttpMethod.Delete, $"/api/roles/{roleId}", null, HttpStatusCode.NoContent);
        var registration = await SendPersistenceJsonAsync(client, admin.AccessToken!, HttpMethod.Post, "/api/auth/register",
            new { username = "security_registration", password = SecurityTestPassword, email = "security-register@example.invalid", firstName = "Registro", lastName = "QA" }, HttpStatusCode.OK);
        Assert.True(registration.GetProperty("success").GetBoolean());
        Assert.Contains(registration.GetProperty("user").GetProperty("permissions").EnumerateArray(),
            value => value.GetString() == PermissionNames.CreateInvoices);
        Assert.Equal(System.Text.Json.JsonValueKind.Null, registration.GetProperty("accessToken").ValueKind);
        var safeguards = new (HttpMethod Method, string Path, object? Body, HttpStatusCode Status)[]
        {
            (HttpMethod.Delete, $"/api/users/{ids.Admin}", null, HttpStatusCode.Conflict),
            (HttpMethod.Put, $"/api/users/{ids.Admin}", new { isActive = false }, HttpStatusCode.Conflict),
            (HttpMethod.Put, $"/api/users/{ids.Admin}", new { roles = Array.Empty<string>() }, HttpStatusCode.Conflict),
            (HttpMethod.Put, $"/api/roles/{ids.AdminRole}", new { permissions = Array.Empty<string>() }, HttpStatusCode.Conflict),
            (HttpMethod.Put, $"/api/roles/{ids.AdminRole}", new { name = "Pretend renamed admin" }, HttpStatusCode.BadRequest),
            (HttpMethod.Delete, $"/api/roles/{ids.AdminRole}", null, HttpStatusCode.BadRequest)
        };
        foreach (var item in safeguards)
            await SendPersistenceStatusAsync(client, full.AccessToken!, item.Method, item.Path, item.Body, item.Status);
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var requiredActions = new[] { "CreateUser", "UpdateUser", "ResetUserPassword", "DeactivateUser", "AssignRole", "CreateRole", "UpdateRole", "DeleteRole", "Register" };
            var actions = await db.AuditLogs.Select(row => row.Action).Distinct().ToListAsync();
            Assert.All(requiredActions, action => Assert.Contains(action, actions));
            var logs = await db.AuditLogs.ToListAsync();
            Assert.DoesNotContain(logs, log => log.Changes is not null && (log.Changes.Contains(SecurityTestPassword)
                || log.Changes.Contains(replacementPassword) || log.Changes.Contains("PasswordHash") || log.Changes.Contains(worker.AccessToken!)));
            Assert.True((await scope.ServiceProvider.GetRequiredService<AuditChainService>().VerifyAsync()).IsValid);
            Assert.True((await db.Users.SingleAsync(row => row.Id == ids.Admin)).IsActive);
            Assert.Equal(PermissionNames.All.Count, await db.RolePermissions.CountAsync(link => link.RoleId == ids.AdminRole));
        }
        var catalogTables = new[] { "Roles", "Permissions", "RolePermissions", "UserRoles" };
        var catalogBefore = await SecurityStateDigestAsync(database.ConnectionString, catalogTables);
        var personalSession = await LoginAsync(client, SecurityTestPassword, "security_worker");
        Assert.Equal(catalogBefore, await SecurityStateDigestAsync(database.ConnectionString, catalogTables));
        const string personalPassword = "SessionIsolation!123456";
        await SendPersistenceStatusAsync(client, personalSession.AccessToken!, HttpMethod.Post, "/api/auth/change-password",
            new { currentPassword = SecurityTestPassword, newPassword = personalPassword }, HttpStatusCode.OK);
        Assert.Equal(catalogBefore, await SecurityStateDigestAsync(database.ConnectionString, catalogTables));
        personalSession = await LoginAsync(client, personalPassword, "security_worker");
        await SendPersistenceStatusAsync(client, personalSession.AccessToken!, HttpMethod.Post, "/api/auth/logout", null, HttpStatusCode.OK);
        Assert.Equal(catalogBefore, await SecurityStateDigestAsync(database.ConnectionString, catalogTables));
        var unchanged = await SecurityStateDigestAsync(database.ConnectionString);
        using (var scope = factory.Services.CreateScope())
        {
            var jwt = scope.ServiceProvider.GetRequiredService<IJwtService>();
            await Assert.ThrowsAsync<InvalidOperationException>(() => jwt.GenerateTokensAsync(ids.Worker));
            await Assert.ThrowsAsync<InvalidOperationException>(() => jwt.ConsumeRefreshTokenAsync(afterRoleDeletion.RefreshToken!));
            await Assert.ThrowsAsync<InvalidOperationException>(() => jwt.RevokeAllRefreshTokensAsync(ids.Worker));
        }
        Assert.Equal(unchanged, await SecurityStateDigestAsync(database.ConnectionString));
        await WriteSecurityMetricsAsync("p02j-operaciones.json", new
        {
            syntheticDatabase = true,
            passed = true,
            auditedMutationKinds = 9,
            administratorSafeguards = safeguards.Length,
            unchangedAndDuplicateRoleSelectionHandled = true,
            userAndRoleRevocation = true,
            credentialsExcludedFromAudit = true,
            auditChainValid = true,
            uncoordinatedJwtCallsRejected = 3,
            sessionWritesLeaveCatalogUnchanged = true, legacyRegistrationPermissionsPreserved = true
        });
    }
}
