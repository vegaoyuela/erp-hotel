using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Auth;
using hotel_erp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static async Task AssertReadAccessMatrixAsync(HttpClient client, IServiceProvider services,
        AuthResponse admin, AuthResponse reception, AuthResponse cashier, AuthResponse accountant)
    {
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var clock = scope.ServiceProvider.GetRequiredService<IHotelClock>();
            var guest = new Guest { FirstName = "Dato", LastName = "Protegido QA" };
            var room = new Room { RoomNumber = "SEC-QA", RoomType = new RoomType { Name = "Seguridad QA", Capacity = 2, PricePerNight = 100 } };
            context.Reservations.Add(new Reservation
            {
                Guest = guest,
                Room = room,
                CheckInDate = clock.Today,
                CheckOutDate = clock.Today.AddDays(1),
                Status = ReservationStatus.Confirmada
            });
            context.Invoices.Add(new Invoice
            {
                CAI = new CAI { CAINumber = "SEC-QA", IssueDate = clock.Today.AddDays(-1), DueDate = clock.Today.AddDays(30), InitialRange = "001-001-01-00000001", FinalRange = "001-001-01-00000100", CurrentCorrelative = "001-001-01-00000002" },
                CorrelativeNumber = "001-001-01-00000001",
                InvoiceDate = clock.HondurasNow,
                CustomerName = "Dato Protegido QA",
                TotalAmount = 100,
                SubTotal = 100,
                ExemptAmount = 100,
                Status = InvoiceStatus.Emitida
            });
            await context.SaveChangesAsync();
        }
        using (var request = Authorized(HttpMethod.Get, "/api/dashboard/summary", admin.AccessToken!))
        using (var response = await client.SendAsync(request))
        {
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Single(document.RootElement.GetProperty("recentInvoices").EnumerateArray());
            Assert.Single(document.RootElement.GetProperty("upcomingReservations").EnumerateArray());
            Assert.NotEqual(JsonValueKind.Null, document.RootElement.GetProperty("cash").ValueKind);
        }
        var profiles = new[] { ("administrator", admin), ("reception", reception), ("cashier", cashier), ("accountant", accountant) };
        var checks = new List<object>();
        // Expectations are explicit business permissions, not inferred from endpoint attributes.
        var cases = new (string Path, bool Reception, bool Cashier, bool Accountant)[]
        {
            ("/api/guests/paged", true, false, true),
            ("/api/guests", true, false, true),
            ("/api/customers/paged", true, false, true),
            ("/api/reservations/paged", true, false, false),
            ("/api/reservations", true, false, false),
            ("/api/rooms", true, false, false),
            ("/api/room-types", true, false, false),
            ("/api/folios/paged", true, true, true),
            ("/api/invoices/paged", true, true, true),
            ("/api/invoices/search", true, true, true),
            ("/api/document-authorizations/options/paged", true, false, true),
            ("/api/cai", true, false, true),
            ("/api/discounts", true, false, false),
            ("/api/tax-configurations", true, false, true),
            ("/api/settings/business", false, false, true),
            ("/api/payments", true, true, true)
        };
        foreach (var (path, allowReception, allowCashier, allowAccountant) in cases)
            foreach (var (name, profile) in profiles)
            {
                var allowed = name == "administrator" || name == "reception" && allowReception
                    || name == "cashier" && allowCashier || name == "accountant" && allowAccountant;
                var expected = allowed ? HttpStatusCode.OK : HttpStatusCode.Forbidden;
                await AssertEndpointStatusAsync(client, profile.AccessToken, path, expected);
                checks.Add(new { role = name, path, status = (int)expected });
            }
        foreach (var (name, profile) in profiles)
            foreach (var action in new[] { "credit-note", "debit-note" })
            {
                using var request = Authorized(HttpMethod.Post, $"/api/invoices/{Guid.NewGuid()}/{action}", profile.AccessToken!);
                request.Content = JsonContent.Create(new { });
                using var response = await client.SendAsync(request);
                var expected = name is "administrator" or "accountant" ? HttpStatusCode.BadRequest : HttpStatusCode.Forbidden;
                Assert.Equal(expected, response.StatusCode);
                checks.Add(new { role = name, action, status = (int)expected });
            }

        string restrictedToken;
        Guid protectedUserId;
        Guid adminRoleId;
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var role = new Role { Name = "Administrador", NormalizedName = "ADMINISTRADOR", Description = "Alias sin privilegios heredados" };
            context.Roles.Add(role);
            foreach (var permission in await context.Permissions.Where(p => p.Name == PermissionNames.ManageUsers || p.Name == PermissionNames.ManageRoles).ToListAsync())
                context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
            var user = new User { Username = "matrix_delegated", Email = "matrix_delegated@example.invalid", FirstName = "QA", LastName = "Delegado", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Matrix!23456"), IsActive = true, MustChangePassword = false };
            context.UserRoles.Add(new UserRole { User = user, Role = role });
            await context.SaveChangesAsync();
            protectedUserId = await context.Users.Where(u => u.Username == "matrix_admin").Select(u => u.Id).SingleAsync();
            adminRoleId = await context.Roles.Where(r => r.SystemKey == SystemRoleKeys.Administrator).Select(r => r.Id).SingleAsync();
        }
        var restricted = await LoginAsync(client, "Matrix!23456", "matrix_delegated");
        restrictedToken = restricted.AccessToken!;
        await AssertEndpointStatusAsync(client, restrictedToken, "/api/invoices/paged", HttpStatusCode.Forbidden);
        await AssertEndpointStatusAsync(client, restrictedToken, "/api/guests/paged", HttpStatusCode.Forbidden);
        await AssertEndpointStatusAsync(client, restrictedToken, $"/api/print/invoice/{Guid.NewGuid()}/preview?secret=must-not-be-audited", HttpStatusCode.Forbidden);
        var deniedChanges = new (HttpMethod Method, string Path, object? Body)[]
        {
            (HttpMethod.Post, $"/api/users/{protectedUserId}/reset-password", new { newPassword = "Changed!987654" }),
            (HttpMethod.Put, $"/api/users/{protectedUserId}", new { firstName = "Unauthorized", roles = Array.Empty<string>() }),
            (HttpMethod.Delete, $"/api/users/{protectedUserId}", null),
            (HttpMethod.Post, $"/api/users/{protectedUserId}/roles/{adminRoleId}", null),
            (HttpMethod.Put, $"/api/roles/{adminRoleId}", new { permissions = Array.Empty<string>() }),
            (HttpMethod.Post, "/api/auth/register", new { username = "forbidden_registration", password = "Changed!987654", email = "forbidden@example.invalid", firstName = "QA", lastName = "Registro" })
        };
        foreach (var (method, path, body) in deniedChanges)
        {
            using var request = Authorized(method, path, restrictedToken);
            if (body is not null) request.Content = JsonContent.Create(body);
            using var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        using (var request = Authorized(HttpMethod.Get, "/api/dashboard/summary", restrictedToken))
        using (var response = await client.SendAsync(request))
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var data = document.RootElement;
            Assert.Empty(data.GetProperty("recentInvoices").EnumerateArray());
            Assert.Empty(data.GetProperty("upcomingReservations").EnumerateArray());
            Assert.Empty(data.GetProperty("stats").EnumerateArray());
            Assert.Empty(data.GetProperty("alerts").EnumerateArray());
            Assert.Equal(JsonValueKind.Null, data.GetProperty("cash").ValueKind);
        }
        using (var request = Authorized(HttpMethod.Get, "/api/dashboard/summary", accountant.AccessToken!))
        using (var response = await client.SendAsync(request))
        {
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(JsonValueKind.Null, document.RootElement.GetProperty("cash").ValueKind);
            Assert.Empty(document.RootElement.GetProperty("upcomingReservations").EnumerateArray());
        }
        // Rejected attempts preserve the protected account, permissions and audit chain.
        var loginStillWorks = await LoginAsync(client, "Matrix!23456", "matrix_admin");
        Assert.True(loginStillWorks.Success);
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.NotEqual("Unauthorized", (await context.Users.SingleAsync(u => u.Id == protectedUserId)).FirstName);
            Assert.Equal(PermissionNames.All.Count, await context.RolePermissions.CountAsync(p => p.RoleId == adminRoleId));
            var denied = await context.AuditLogs.Where(row => row.Action == "AccessDenied").ToListAsync();
            Assert.NotEmpty(denied);
            Assert.DoesNotContain(denied, row => row.Changes != null && (row.Changes.Contains("must-not-be-audited") || row.Changes.Contains("secret=")));
            Assert.Equal(6, await context.AuditLogs.CountAsync(row => row.Action == "SecurityChangeDenied"));
        }
        Guid pageRoleId;
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var permission = await context.Permissions.SingleAsync(p => p.Name == PermissionNames.ManageUsers);
            var sharedRole = new Role { Name = "PAGE-QA-Shared", NormalizedName = "PAGE-QA-SHARED" };
            for (var index = 0; index < 31; index++)
            {
                var role = new Role { Name = $"PAGE-QA-Role-{index:D2}", NormalizedName = $"PAGE-QA-ROLE-{index:D2}" };
                context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                var user = new User { Username = $"page_qa_{index:D2}", Email = $"page_qa_{index:D2}@example.invalid", FirstName = "Página", LastName = $"QA {index:D2}", PasswordHash = "not-a-login-credential", IsActive = false };
                context.UserRoles.Add(new UserRole { User = user, Role = sharedRole });
            }
            await context.SaveChangesAsync();
            pageRoleId = sharedRole.Id;
        }
        async Task<JsonElement> Page(string path) => await SendPersistenceJsonAsync(client, restrictedToken,
            HttpMethod.Get, path, null, HttpStatusCode.OK);
        var pageOne = await Page($"/api/users/paged?search=page_qa&roleId={pageRoleId}&pageSize=10");
        var pageTwo = await Page($"/api/users/paged?search=page_qa&roleId={pageRoleId}&pageSize=10&page=2");
        Assert.Equal(31, pageOne.GetProperty("totalItems").GetInt32());
        Assert.Equal(10, pageOne.GetProperty("items").GetArrayLength());
        var firstIds = pageOne.GetProperty("items").EnumerateArray().Select(row => row.GetProperty("id").GetGuid()).ToHashSet();
        Assert.DoesNotContain(pageTwo.GetProperty("items").EnumerateArray(), row => firstIds.Contains(row.GetProperty("id").GetGuid()));
        var options = await Page("/api/users/role-options/paged?search=PAGE-QA-Role&assignableOnly=true&pageSize=10&page=2");
        Assert.Equal(31, options.GetProperty("totalItems").GetInt32());
        Assert.All(options.GetProperty("items").EnumerateArray(), row => Assert.Equal(2, row.EnumerateObject().Count()));
        var protectedRoleOptions = await Page("/api/users/role-options/paged?search=Admin&assignableOnly=true");
        Assert.DoesNotContain(protectedRoleOptions.GetProperty("items").EnumerateArray(), row => row.GetProperty("id").GetGuid() == adminRoleId);
        foreach (var path in new[] { "/api/users/paged?pageSize=101", "/api/users/paged?roleId=invalid", "/api/users/role-options/paged?assignableOnly=invalid" })
        {
            using var request = Authorized(HttpMethod.Get, path, restrictedToken);
            using var response = await client.SendAsync(request);
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.BadRequest);
        }
        var output = Environment.GetEnvironmentVariable("HOTEL_ERP_READ_ACCESS_METRICS");
        if (!string.IsNullOrWhiteSpace(output)) await File.WriteAllTextAsync(output, JsonSerializer.Serialize(new
        {
            syntheticDatabase = true,
            readChecks = checks,
            pagedUsers = 31,
            pagedRoleOptions = 31,
            rejectedPrivilegedAccountChanges = deniedChanges.Length,
            roleLabelsGrantAccess = false,
            dashboardMinimization = true,
            deniedRequestsAudited = true,
            auditContainsRequestSecrets = false
        }, new JsonSerializerOptions { WriteIndented = true }));
    }
}
