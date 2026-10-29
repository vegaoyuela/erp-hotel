using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private const string SecurityTestPassword = "Matrix!23456";
    private sealed record SecurityTestIds(Guid Admin, Guid Worker, Guid WorkerRole, Guid ExtraRole,
        Guid Delegate, Guid DelegateRole, Guid FullDelegate, Guid SecondAdmin, Guid AdminRole);

    private static Dictionary<string, string?> SecurityTestEnvironment(string connection) => new()
    {
        ["ConnectionStrings__DefaultConnection"] = connection,
        ["Jwt__SecretKey"] = "integration-only-secret-key-with-at-least-32-bytes",
        ["Jwt__Issuer"] = "hotel-erp-integration",
        ["Jwt__Audience"] = "hotel-erp-integration",
        ["Cors__AllowedOrigins__0"] = "http://localhost",
        ["BootstrapAdmin__Username"] = "integration_admin",
        ["BootstrapAdmin__Email"] = "integration_admin@example.invalid",
        ["BootstrapAdmin__Password"] = "Integration!234",
        ["AuthenticationRateLimiting__LoginPermitLimit"] = "1000"
    };

    private static HotelErpWebApplicationFactory SecurityFactory(string connection, SecurityTestInjector injector)
        => new(connection, services =>
        {
            services.RemoveAll<IApplicationFailureInjector>();
            services.AddSingleton<IApplicationFailureInjector>(injector);
        });

    private static async Task<SecurityTestIds> SeedSecurityActorsAsync(IServiceProvider services)
    {
        await SeedRoleMatrixUsersAsync(services);
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        (await db.Users.SingleAsync(row => row.Username == "integration_admin")).IsActive = false;
        var admin = await db.Users.SingleAsync(row => row.Username == "matrix_admin");
        var adminRole = await db.Roles.SingleAsync(row => row.SystemKey == SystemRoleKeys.Administrator);
        var all = await db.Permissions.ToDictionaryAsync(row => row.Name);
        Role Role(string name, params string[] permissions)
        {
            var role = new Role { Name = name, NormalizedName = name.ToUpperInvariant() };
            db.Roles.Add(role);
            foreach (var permission in permissions) db.RolePermissions.Add(new RolePermission { Role = role, Permission = all[permission] });
            return role;
        }
        User User(string name, Role role, bool active = true)
        {
            var user = new User
            {
                Username = name,
                Email = name + "@example.invalid",
                FirstName = "Seguridad",
                LastName = "QA",
                PasswordHash = admin.PasswordHash,
                IsActive = active,
                MustChangePassword = false
            };
            db.UserRoles.Add(new UserRole { User = user, Role = role });
            return user;
        }
        var workerRole = Role("Security QA Worker", PermissionNames.ViewReports);
        var worker = User("security_worker", workerRole);
        var extra = Role("Security QA Extra", PermissionNames.ManageInventory);
        var delegatedRole = Role("Security QA Delegate", PermissionNames.ManageRoles);
        var delegated = User("security_delegate", delegatedRole);
        var full = User("security_full_delegate", Role("Security QA Full", PermissionNames.All.ToArray()));
        var second = User("security_second_admin", adminRole, false);
        await db.SaveChangesAsync();
        return new(admin.Id, worker.Id, workerRole.Id, extra.Id, delegated.Id, delegatedRole.Id, full.Id, second.Id, adminRole.Id);
    }

    private sealed class InjectedSecurityFailure : Exception { }
    private sealed class SecurityTestInjector : IApplicationFailureInjector
    {
        public string? FailAt { get; set; }
        private int _hold;
        private TaskCompletionSource _entered = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private TaskCompletionSource _release = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public Task Entered => _entered.Task;
        public void HoldNextAdministration()
        {
            _entered = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _release = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _hold = 1;
        }
        public void Release() => _release.TrySetResult();
        public async Task CheckpointAsync(string checkpoint, CancellationToken cancellationToken = default)
        {
            if (checkpoint == ApplicationFailureCheckpoints.SecurityAfterLock && Interlocked.Exchange(ref _hold, 0) == 1)
            {
                _entered.TrySetResult();
                await _release.Task.WaitAsync(TimeSpan.FromSeconds(20), cancellationToken);
            }
            if (checkpoint == FailAt) throw new InjectedSecurityFailure();
        }
    }

    private static async Task<HttpResponseMessage> SecuritySendAsync(HttpClient client, string? token, HttpMethod method, string path, object? body = null)
    {
        using var request = new HttpRequestMessage(method, path);
        if (token is not null) request.Headers.Authorization = new("Bearer", token);
        if (body is not null) request.Content = JsonContent.Create(body);
        return await client.SendAsync(request);
    }

    private static async Task<string> SecurityStateDigestAsync(string connection, params string[] selectedTables)
    {
        await using var db = new NpgsqlConnection(connection);
        await db.OpenAsync();
        var value = new StringBuilder();
        foreach (var table in selectedTables.Length > 0 ? selectedTables : new[] { "Users", "Roles", "Permissions", "UserRoles", "RolePermissions", "RefreshTokens", "AuditLogs", "CorrelativeLocks" })
        {
            await using var command = db.CreateCommand();
            command.CommandText = $"SELECT COALESCE(jsonb_agg(to_jsonb(t) ORDER BY to_jsonb(t)::text), '[]'::jsonb)::text FROM \"{table}\" t";
            value.Append(await command.ExecuteScalarAsync());
        }
        // Only the digest is compared or displayed on failure, never credential/session columns.
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value.ToString())));
    }

    private static async Task WaitForSecurityLockWaiterAsync(string connection)
    {
        await using var db = new NpgsqlConnection(connection);
        await db.OpenAsync();
        var deadline = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < deadline)
        {
            await using var command = db.CreateCommand();
            command.CommandText = "SELECT EXISTS(SELECT 1 FROM pg_locks l JOIN pg_stat_activity a ON a.pid=l.pid WHERE a.datname=current_database() AND l.locktype='advisory' AND NOT l.granted)";
            if ((bool)(await command.ExecuteScalarAsync())!) return;
            await Task.Delay(25);
        }
        Assert.Fail("La segunda solicitud no llegó al bloqueo de seguridad dentro del plazo.");
    }

    private static async Task WriteSecurityMetricsAsync(string name, object value)
    {
        var directory = Environment.GetEnvironmentVariable("HOTEL_ERP_SECURITY_METRICS_DIRECTORY");
        if (!string.IsNullOrWhiteSpace(directory)) await File.WriteAllTextAsync(Path.Combine(directory, name),
            JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true }));
    }
}
