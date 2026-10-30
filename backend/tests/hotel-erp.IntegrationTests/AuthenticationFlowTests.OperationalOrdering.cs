using System.Net;
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
    public async Task OperationalMutations_FinishBeforeRevocationAndReleaseOnCancellation()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        await using var database = new TemporaryPostgreSqlDatabase(Environment.GetEnvironmentVariable("HOTEL_ERP_TEST_ADMIN_CONNECTION")!);
        await database.CreateAsync();
        using var environment = new EnvironmentVariableScope(SecurityTestEnvironment(database.ConnectionString));
        var injector = new OperationalRaceInjector();
        await using var factory = OperationalFactory(database.ConnectionString, injector);
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var ids = await SeedSecurityActorsAsync(factory.Services);
        var productId = await PrepareOperationalWorkerAsync(factory.Services, ids);
        var admin = await LoginAsync(client, SecurityTestPassword, "matrix_admin");
        var worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        var movement = new { productId, movementType = "Entrada", quantity = 1, unitPrice = 100m };
        async Task<int> Stock()
        {
            using var scope = factory.Services.CreateScope();
            return await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Products.Where(row => row.Id == productId).Select(row => row.CurrentStock).SingleAsync();
        }
        async Task<NpgsqlTransaction> LockProduct(NpgsqlConnection connection)
        {
            var tx = await connection.BeginTransactionAsync();
            await using var command = connection.CreateCommand();
            command.Transaction = tx;
            command.CommandText = "SELECT pg_advisory_xact_lock(hashtext(@key))";
            command.Parameters.AddWithValue("key", $"inventory-product:{productId:N}");
            await command.ExecuteNonQueryAsync();
            return tx;
        }
        await using (var blocker = new NpgsqlConnection(database.ConnectionString))
        {
            await blocker.OpenAsync();
            await using var resource = await LockProduct(blocker);
            var operation = SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post, "/api/inventory/movements", movement);
            await WaitForOperationalWaitersAsync(database.ConnectionString, 1);
            var revocation = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Put, $"/api/roles/{ids.WorkerRole}", new { permissions = Array.Empty<string>() });
            try { await WaitForOperationalWaitersAsync(database.ConnectionString, 2); }
            finally { await resource.RollbackAsync(); }
            using (var response = await operation) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using (var response = await revocation) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        Assert.Equal(11, await Stock());
        await AssertEndpointStatusAsync(client, worker.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);

        await PrepareOperationalWorkerAsync(factory.Services, ids, restore: true);
        worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        await using (var blocker = new NpgsqlConnection(database.ConnectionString))
        {
            await blocker.OpenAsync();
            await using var resource = await LockProduct(blocker);
            var operation = SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post, "/api/inventory/movements", movement);
            await WaitForOperationalWaitersAsync(database.ConnectionString, 1);
            var logout = SecuritySendAsync(client, worker.AccessToken, HttpMethod.Post, "/api/auth/logout");
            try { await WaitForOperationalWaitersAsync(database.ConnectionString, 2); }
            finally { await resource.RollbackAsync(); }
            using (var response = await operation) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using (var response = await logout) Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        Assert.Equal(12, await Stock());
        await AssertEndpointStatusAsync(client, worker.AccessToken, "/api/auth/me", HttpStatusCode.Unauthorized);

        worker = await LoginAsync(client, SecurityTestPassword, "security_worker");
        injector.HoldNextOperation();
        using var cancellation = new CancellationTokenSource();
        using var request = Authorized(HttpMethod.Post, "/api/inventory/movements", worker.AccessToken!);
        request.Content = System.Net.Http.Json.JsonContent.Create(movement);
        var canceledOperation = client.SendAsync(request, cancellation.Token);
        await injector.Entered.WaitAsync(TimeSpan.FromSeconds(10));
        var waitingRevoke = SecuritySendAsync(client, admin.AccessToken, HttpMethod.Put, $"/api/roles/{ids.WorkerRole}", new { permissions = Array.Empty<string>() });
        try
        {
            await WaitForOperationalWaitersAsync(database.ConnectionString, 1);
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => canceledOperation);
        }
        finally { injector.Release(); }
        using (var response = await waitingRevoke) Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(12, await Stock());
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(2, await db.InventoryMovements.CountAsync(row => row.ProductId == productId));
            Assert.True((await scope.ServiceProvider.GetRequiredService<AuditChainService>().VerifyAsync()).IsValid);
        }
        await WriteSecurityMetricsAsync("p02k-orden.json", new
        {
            syntheticDatabase = true,
            passed = true,
            activeOperationBeforeRoleRevocation = true,
            activeOperationBeforeOwnLogout = true,
            canceledAdmissionReleasesSecurityLocks = true,
            resourceAndSecurityWaitersObserved = true,
            initialStock = 10,
            committedMovements = 2,
            finalStock = 12,
            canceledMovements = 0,
            auditChainValid = true
        });
    }
}
