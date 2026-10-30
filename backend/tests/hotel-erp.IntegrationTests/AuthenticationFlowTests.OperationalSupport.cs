using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static readonly Type[] OperationalServices = [typeof(InvoiceApplicationService), typeof(PaymentApplicationService),
        typeof(RefundApplicationService), typeof(CardSettlementApplicationService), typeof(ReservationApplicationService),
        typeof(FolioApplicationService), typeof(CashRegisterApplicationService), typeof(PurchaseInvoiceApplicationService), typeof(InventoryApplicationService)];
    private static readonly string[] OperationalPermissions = [PermissionNames.CreateInvoices, PermissionNames.AdjustInvoices,
        PermissionNames.ManageCash, PermissionNames.ManageAccounting, PermissionNames.ManageReservations, PermissionNames.ManageInventory];

    private sealed class OperationalRaceInjector : IApplicationFailureInjector
    {
        public SecurityTestInjector Administration { get; } = new();
        private int _hold;
        private TaskCompletionSource _entered = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private TaskCompletionSource _release = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public Task Entered => _entered.Task;
        public void HoldNextOperation()
        {
            _entered = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _release = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _hold = 1;
        }
        public void Release() => _release.TrySetResult();
        public async Task CheckpointAsync(string checkpoint, CancellationToken cancellationToken = default)
        {
            await Administration.CheckpointAsync(checkpoint, cancellationToken);
            if (checkpoint == ApplicationFailureCheckpoints.OperationalAfterAdmission && Interlocked.Exchange(ref _hold, 0) == 1)
            {
                _entered.TrySetResult();
                await _release.Task.WaitAsync(TimeSpan.FromSeconds(20), cancellationToken);
            }
        }
    }

    private static HotelErpWebApplicationFactory OperationalFactory(string connection, OperationalRaceInjector injector)
        => new(connection, services =>
        {
            services.RemoveAll<IApplicationFailureInjector>();
            services.AddSingleton<IApplicationFailureInjector>(injector);
        });

    private static async Task<Guid> PrepareOperationalWorkerAsync(IServiceProvider services, SecurityTestIds ids, bool restore = false)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var existing = await db.RolePermissions.Where(link => link.RoleId == ids.WorkerRole).Select(link => link.PermissionId).ToArrayAsync();
        foreach (var permission in await db.Permissions.Where(row => OperationalPermissions.Contains(row.Name)).ToListAsync())
            if (!existing.Contains(permission.Id)) db.RolePermissions.Add(new RolePermission { RoleId = ids.WorkerRole, PermissionId = permission.Id });
        if (restore) (await db.Users.SingleAsync(row => row.Id == ids.Worker)).SecurityVersion++;
        var product = await db.Products.FirstOrDefaultAsync(row => row.Name == "Operational lock QA");
        if (product is null)
        {
            product = new Product { Name = "Operational lock QA", CurrentStock = 10, UnitPrice = 100, IsActive = true };
            db.Products.Add(product);
        }
        await db.SaveChangesAsync();
        return product.Id;
    }

    private static async Task<SecurityActor> OperationalActorAsync(IServiceProvider services, Guid id)
    {
        using var scope = services.CreateScope();
        return new(id, await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Users.Where(row => row.Id == id)
            .Select(row => row.SecurityVersion).SingleAsync());
    }

    private static async Task WaitForOperationalWaitersAsync(string connection, int count)
    {
        await using var db = new NpgsqlConnection(connection);
        await db.OpenAsync();
        var deadline = DateTime.UtcNow.AddSeconds(12);
        while (DateTime.UtcNow < deadline)
        {
            await using var command = db.CreateCommand();
            command.CommandText = "SELECT count(*) FROM pg_locks l JOIN pg_stat_activity a ON a.pid=l.pid WHERE a.datname=current_database() AND l.locktype='advisory' AND NOT l.granted";
            if ((long)(await command.ExecuteScalarAsync())! >= count) return;
            await Task.Delay(25);
        }
        Assert.Fail($"No se observaron las {count} esperas de bloqueo requeridas.");
    }

    private static async Task<string> OperationalBusinessDigestAsync(string connection)
    {
        await using var db = new NpgsqlConnection(connection);
        await db.OpenAsync();
        var excluded = new[] { "Users", "Roles", "Permissions", "UserRoles", "RolePermissions", "RefreshTokens", "AuditLogs", "CorrelativeLocks", "__EFMigrationsHistory" };
        var tables = new List<string>();
        await using (var command = db.CreateCommand())
        {
            command.CommandText = "SELECT tablename FROM pg_tables WHERE schemaname='public' ORDER BY tablename";
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) if (!excluded.Contains(reader.GetString(0))) tables.Add(reader.GetString(0));
        }
        var state = new StringBuilder();
        foreach (var table in tables)
        {
            await using var command = db.CreateCommand();
            command.CommandText = $"SELECT COALESCE(jsonb_agg(to_jsonb(t) ORDER BY to_jsonb(t)::text),'[]'::jsonb)::text FROM {new NpgsqlCommandBuilder().QuoteIdentifier(table)} t";
            state.Append(await command.ExecuteScalarAsync());
        }
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(state.ToString())));
    }

    private static async Task AssertDirectOperationalDeniedAsync(IServiceProvider services, Type service, MethodInfo method, SecurityActor actor, int status)
    {
        using var scope = services.CreateScope();
        var arguments = method.GetParameters().Select(parameter => parameter.ParameterType == typeof(SecurityActor) ? (object)actor
            : parameter.ParameterType == typeof(CancellationToken) ? CancellationToken.None
            : parameter.ParameterType == typeof(Guid) ? Guid.NewGuid()
            : parameter.ParameterType == typeof(string) ? Guid.NewGuid().ToString("D") : null).ToArray();
        // Null business DTOs deliberately prove authorization happens before any business input is consumed.
        var task = (Task)method.Invoke(scope.ServiceProvider.GetRequiredService(service), arguments)!;
        var error = await Assert.ThrowsAsync<OperationalAccessException>(() => task);
        Assert.Equal(status, error.Status);
    }
}
