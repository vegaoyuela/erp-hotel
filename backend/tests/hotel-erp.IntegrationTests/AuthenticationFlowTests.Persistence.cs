using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static async Task AssertExplicitPersistenceConsumersAsync(
        HttpClient client,
        IServiceProvider services,
        string accessToken)
    {
        var simpleCases = new[]
        {
            (Path: "/api/customers", Create: (object)new { name = "Cliente persistencia" },
                Update: (object)new { name = "Cliente actualizado" }, Field: "name", Expected: "Cliente actualizado"),
            (Path: "/api/guests", Create: (object)new { firstName = "Huésped", lastName = "Persistencia" },
                Update: (object)new { lastName = "Actualizado" }, Field: "lastName", Expected: "Actualizado"),
            (Path: "/api/suppliers", Create: (object)new { name = "Proveedor persistencia" },
                Update: (object)new { name = "Proveedor actualizado" }, Field: "name", Expected: "Proveedor actualizado"),
            (Path: "/api/discounts", Create: (object)new { name = "Descuento persistencia", value = 5m },
                Update: (object)new { name = "Descuento actualizado" }, Field: "name", Expected: "Descuento actualizado"),
            (Path: "/api/room-types", Create: (object)new { name = "Tipo persistencia", pricePerNight = 150m, capacity = 2 },
                Update: (object)new { name = "Tipo actualizado" }, Field: "name", Expected: "Tipo actualizado")
        };

        foreach (var item in simpleCases)
        {
            var created = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Post, item.Path,
                item.Create, HttpStatusCode.Created);
            var id = created.GetProperty("id").GetGuid();
            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Put, $"{item.Path}/{id}",
                item.Update, HttpStatusCode.NoContent);
            var fetched = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get, $"{item.Path}/{id}",
                null, HttpStatusCode.OK);
            Assert.Equal(item.Expected, fetched.GetProperty(item.Field).GetString());
            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Delete, $"{item.Path}/{id}",
                null, HttpStatusCode.NoContent);
            await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Get, $"{item.Path}/{id}",
                null, HttpStatusCode.NotFound);
        }

        Guid roomTypeId;
        using (var scope = services.CreateScope())
        {
            roomTypeId = await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
                .RoomTypes.Select(roomType => roomType.Id).FirstAsync();
        }
        var room = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Post, "/api/rooms",
            new { roomNumber = "PERSIST", floor = 1, roomTypeId }, HttpStatusCode.Created);
        var roomId = room.GetProperty("id").GetGuid();
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Put, $"/api/rooms/{roomId}",
            new { observations = "Guardado explícito" }, HttpStatusCode.NoContent);
        var updatedRoom = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get, $"/api/rooms/{roomId}",
            null, HttpStatusCode.OK);
        Assert.Equal("Guardado explícito", updatedRoom.GetProperty("observations").GetString());
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Delete, $"/api/rooms/{roomId}",
            null, HttpStatusCode.NoContent);
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Get, $"/api/rooms/{roomId}",
            null, HttpStatusCode.NotFound);

        var account = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Post, "/api/accounts",
            new { accountNumber = "9901", accountName = "Cuenta persistencia", accountType = "Activo" },
            HttpStatusCode.Created);
        var accountId = account.GetProperty("id").GetGuid();
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Put, $"/api/accounts/{accountId}",
            new { accountName = "Cuenta actualizada" }, HttpStatusCode.NoContent);
        var updatedAccount = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get,
            $"/api/accounts/{accountId}", null, HttpStatusCode.OK);
        Assert.Equal("Cuenta actualizada", updatedAccount.GetProperty("accountName").GetString());
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Delete, $"/api/accounts/{accountId}",
            null, HttpStatusCode.OK);
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Get, $"/api/accounts/{accountId}",
            null, HttpStatusCode.NotFound);

        // Role creation must persist the new role and its permissions in one flush.
        var role = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Post, "/api/roles",
            new { name = "Rol persistencia", permissions = new[] { PermissionNames.ViewReports, PermissionNames.ManageInventory } },
            HttpStatusCode.Created);
        var roleId = role.GetProperty("id").GetGuid();
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Put, $"/api/roles/{roleId}",
            new { description = "Actualizado", permissions = new[] { PermissionNames.ViewReports } },
            HttpStatusCode.NoContent);
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var persistedRole = await context.Roles.Include(candidate => candidate.RolePermissions)
                .ThenInclude(candidate => candidate.Permission).SingleAsync(candidate => candidate.Id == roleId);
            Assert.Equal("Actualizado", persistedRole.Description);
            Assert.Equal(PermissionNames.ViewReports, Assert.Single(persistedRole.RolePermissions).Permission.Name);
        }
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Delete, $"/api/roles/{roleId}",
            null, HttpStatusCode.NoContent);
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Get, $"/api/roles/{roleId}",
            null, HttpStatusCode.NotFound);

        var password = $"Qa!{Guid.NewGuid():N}aA1";
        var userName = $"qa_{Guid.NewGuid():N}"[..24];
        var user = await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Post, "/api/users",
            new { username = userName, password, email = $"{userName}@example.invalid", firstName = "Usuario", lastName = "Persistencia" },
            HttpStatusCode.Created);
        var userId = user.GetProperty("id").GetGuid();
        using (var failedLogin = await client.PostAsJsonAsync("/api/auth/login", new { username = userName, password = "incorrecta" }))
            Assert.Equal(HttpStatusCode.Unauthorized, failedLogin.StatusCode);
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(1, (await context.Users.SingleAsync(candidate => candidate.Id == userId)).FailedLoginAttempts);
        }
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Post, $"/api/users/{userId}/reset-password",
            new { newPassword = $"Qa!{Guid.NewGuid():N}aA1" }, HttpStatusCode.OK);
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var persistedUser = await context.Users.SingleAsync(candidate => candidate.Id == userId);
            Assert.Equal(0, persistedUser.FailedLoginAttempts);
            Assert.True(persistedUser.MustChangePassword);
            Assert.False(BCrypt.Net.BCrypt.Verify(password, persistedUser.PasswordHash));
        }
        await SendPersistenceStatusAsync(client, accessToken, HttpMethod.Delete, $"/api/users/{userId}", null, HttpStatusCode.OK);
        using (var scope = services.CreateScope())
            Assert.False((await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
                .Users.SingleAsync(candidate => candidate.Id == userId)).IsActive);
    }

    private static async Task<JsonElement> SendPersistenceJsonAsync(
        HttpClient client, string accessToken, HttpMethod method, string path, object? body, HttpStatusCode expected)
    {
        using var request = Authorized(method, path, accessToken);
        if (body is not null) request.Content = JsonContent.Create(body);
        using var response = await client.SendAsync(request);
        var text = await response.Content.ReadAsStringAsync();
        Assert.True(response.StatusCode == expected, $"{method} {path}: {(int)response.StatusCode} {text}");
        using var json = JsonDocument.Parse(text);
        return json.RootElement.Clone();
    }

    private static async Task SendPersistenceStatusAsync(
        HttpClient client, string accessToken, HttpMethod method, string path, object? body, HttpStatusCode expected)
    {
        using var request = Authorized(method, path, accessToken);
        if (body is not null) request.Content = JsonContent.Create(body);
        using var response = await client.SendAsync(request);
        Assert.True(response.StatusCode == expected,
            $"{method} {path}: {(int)response.StatusCode} {await response.Content.ReadAsStringAsync()}");
    }
}
