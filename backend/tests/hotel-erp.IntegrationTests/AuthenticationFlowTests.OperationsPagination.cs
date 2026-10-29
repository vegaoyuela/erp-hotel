using System.Net;
using System.Text;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_erp.IntegrationTests;

public partial class AuthenticationFlowTests
{
    private static async Task AssertOperationsPaginationAsync(HttpClient client, IServiceProvider services, string accessToken)
    {
        const string marker = "Operación QA-H2";
        Guid categoryId;
        Guid productId;
        Guid historicalFolioId;
        int expectedCategoryProducts;
        int expectedTotalProducts;
        int expectedActiveProducts;
        int expectedLowProducts;
        int expectedMovements;
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var room = new Room { RoomNumber = "H2-QA", RoomType = new RoomType { Name = marker, Capacity = 2, PricePerNight = 100m } };
            var guest = new Guest { FirstName = marker, LastName = "Huésped", DocumentNumber = "QA-H2-DOC" };
            var category = new Category { Name = marker };
            var otherCategory = new Category { Name = "Otra categoría QA-H2" };
            context.Rooms.Add(room);
            context.Guests.Add(guest);
            context.Categories.AddRange(category, otherCategory);
            var products = Enumerable.Range(0, 31).Select(index => new Product
            {
                Name = marker,
                SKU = $"QA-H2-{index:D2}",
                Description = $"Detalle {index:D2}",
                Category = index % 2 == 0 ? category : otherCategory,
                IsActive = index % 3 != 0,
                CurrentStock = index * 2,
                MinStockLevel = 10,
                UnitPrice = 5m
            }).ToList();
            context.Products.AddRange(products);
            context.Customers.AddRange(Enumerable.Range(0, 31).Select(index => new Customer
            {
                Name = marker,
                Email = $"qa-h2-{index:D2}@example.invalid",
                Phone = $"QA-H2-{index:D2}"
            }));
            for (var index = 0; index < 31; index++)
            {
                var reservation = new Reservation
                {
                    Room = room,
                    Guest = guest,
                    CheckInDate = new DateOnly(2036, 1, 1).AddDays(index * 2),
                    CheckOutDate = new DateOnly(2036, 1, 2).AddDays(index * 2),
                    Status = index % 2 == 0 ? ReservationStatus.Confirmada : ReservationStatus.Cancelada
                };
                context.Folios.Add(new Folio
                {
                    Reservation = reservation,
                    Guest = guest,
                    Room = room,
                    TotalAmount = 100m,
                    OpeningDate = new DateTime(2036, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                    Status = index % 2 == 0 ? FolioStatus.Abierto : FolioStatus.Cerrado,
                    FolioItems = [new() { Description = "Cargo visible", Quantity = 1, UnitPrice = 100m, LineTotal = 100m },
                        new() { Description = "Cargo eliminado", IsDeleted = true }]
                });
            }
            var historicalGuest = new Guest { FirstName = "Histórico QA-H2", LastName = "Conservado", IsDeleted = true };
            var historicalReservation = new Reservation
            {
                Guest = historicalGuest,
                Room = room,
                Status = ReservationStatus.CheckOut,
                CheckInDate = new(2035, 1, 1),
                CheckOutDate = new(2035, 1, 2)
            };
            var historical = new Folio
            {
                Guest = historicalGuest,
                Room = room,
                Reservation = historicalReservation,
                Status = FolioStatus.Cerrado
            };
            context.Folios.Add(historical);
            context.InventoryMovements.AddRange(Enumerable.Range(0, 31).Select(index => new InventoryMovement
            {
                Product = products[0],
                MovementType = index % 2 == 0 ? InventoryMovementType.Entrada : InventoryMovementType.Salida,
                Quantity = 1,
                NewStock = index,
                MovementDate = new DateTime(2036, 1, 1, 12, 0, 0, DateTimeKind.Utc)
            }));
            await context.SaveChangesAsync();
            categoryId = category.Id;
            productId = products[0].Id;
            historicalFolioId = historical.Id;
            expectedCategoryProducts = products.Count(product => product.CategoryId == categoryId && product.IsActive);
            expectedTotalProducts = await context.Products.CountAsync();
            expectedActiveProducts = await context.Products.CountAsync(product => product.IsActive);
            expectedLowProducts = await context.Products.CountAsync(product => product.IsActive && product.CurrentStock <= product.MinStockLevel);
            expectedMovements = await context.InventoryMovements.CountAsync();
        }

        async Task<JsonElement> Read(string path)
            => await SendPersistenceJsonAsync(client, accessToken, HttpMethod.Get, path, null, HttpStatusCode.OK);
        var search = Uri.EscapeDataString(marker);
        var metrics = new List<object>();
        foreach (var (path, legacyPath, expected) in new[]
        {
            ($"/api/reservations/paged?search={search}", "/api/reservations", 31),
            ($"/api/folios/paged?search={search}", "/api/folios", 31),
            ($"/api/customers/paged?search={search}", $"/api/customers?search={search}", 31),
            ($"/api/inventory/products/paged?search={search}", $"/api/inventory/products?search={search}", 31),
            ($"/api/inventory/movements/paged?productId={productId}", $"/api/inventory/movements?productId={productId}", 31)
        })
        {
            var first = await Read(path + "&pageSize=10&page=1");
            var second = await Read(path + "&pageSize=10&page=2");
            var last = await Read(path + "&pageSize=10&page=99");
            var repeated = await Read(path + "&pageSize=10&page=1");
            Assert.Equal(expected, first.GetProperty("totalItems").GetInt32());
            Assert.Equal(10, first.GetProperty("items").GetArrayLength());
            Assert.Equal(4, last.GetProperty("page").GetInt32());
            Assert.Single(last.GetProperty("items").EnumerateArray());
            var firstIds = first.GetProperty("items").EnumerateArray().Select(item => item.GetProperty("id").GetGuid()).ToArray();
            Assert.Equal(firstIds, repeated.GetProperty("items").EnumerateArray().Select(item => item.GetProperty("id").GetGuid()));
            Assert.DoesNotContain(second.GetProperty("items").EnumerateArray(), item => firstIds.Contains(item.GetProperty("id").GetGuid()));
            var legacy = await Read(legacyPath);
            metrics.Add(new
            {
                path,
                matchedRows = expected,
                legacyRows = legacy.GetArrayLength(),
                pageRows = 10,
                legacyBytes = Encoding.UTF8.GetByteCount(legacy.GetRawText()),
                pageBytes = Encoding.UTF8.GetByteCount(first.GetRawText())
            });
            using var anonymous = await client.GetAsync(path);
            Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);
        }
        var reservations = await Read($"/api/reservations/paged?search={search}&status=Confirmada");
        Assert.Equal(16, reservations.GetProperty("totalItems").GetInt32());
        var folios = await Read($"/api/folios/paged?search={search}&status=Abierto");
        Assert.Equal(16, folios.GetProperty("totalItems").GetInt32());
        Assert.All(folios.GetProperty("items").EnumerateArray(), item =>
        {
            Assert.Equal(1, item.GetProperty("itemCount").GetInt32());
            Assert.False(item.TryGetProperty("items", out _));
        });
        var historicalResult = await Read("/api/folios/paged?search=Hist%C3%B3rico%20QA-H2");
        Assert.Equal(historicalFolioId, historicalResult.GetProperty("items")[0].GetProperty("id").GetGuid());
        var customerByEmail = await Read("/api/customers/paged?search=qa-h2-30%40example.invalid");
        Assert.Equal(1, customerByEmail.GetProperty("totalItems").GetInt32());
        var productsResult = await Read($"/api/inventory/products/paged?search={search}&categoryId={categoryId}&isActive=true");
        Assert.Equal(expectedCategoryProducts, productsResult.GetProperty("totalItems").GetInt32());
        var movementsResult = await Read($"/api/inventory/movements/paged?productId={productId}&movementType=Entrada");
        Assert.Equal(16, movementsResult.GetProperty("totalItems").GetInt32());
        var summary = await Read("/api/inventory/summary");
        Assert.Equal(expectedTotalProducts, summary.GetProperty("totalProducts").GetInt32());
        Assert.Equal(expectedActiveProducts, summary.GetProperty("activeProducts").GetInt32());
        Assert.Equal(expectedLowProducts, summary.GetProperty("lowStockProducts").GetInt32());
        Assert.Equal(expectedMovements, summary.GetProperty("totalMovements").GetInt32());
        foreach (var path in new[] { "/api/reservations/paged?status=Invalido", "/api/folios/paged?pageSize=101",
            "/api/inventory/products/paged?categoryId=invalid", "/api/inventory/movements/paged?movementType=99" })
        {
            using var request = Authorized(HttpMethod.Get, path, accessToken);
            using var response = await client.SendAsync(request);
            await AssertSafeProblemDetailsAsync(response, HttpStatusCode.BadRequest);
        }
        var empty = await Read("/api/customers/paged?search=never-existing-QA-H2");
        Assert.Equal(0, empty.GetProperty("totalItems").GetInt32());
        using var schemaResponse = await client.GetAsync("/openapi/v1.json");
        using var schema = JsonDocument.Parse(await schemaResponse.Content.ReadAsStringAsync());
        var paths = schema.RootElement.GetProperty("paths");
        foreach (var (path, expectedSchema) in new[]
        {
            ("/api/Reservations/checkin", "CheckInResult"),
            ("/api/Reservations/checkout/{id}", "CheckOutResult"),
            ("/api/Folios/{folioId}/items", "FolioChargeResult")
        })
        {
            var responses = paths.GetProperty(path).GetProperty("post").GetProperty("responses");
            Assert.EndsWith("/" + expectedSchema, responses.GetProperty("200").GetProperty("content")
                .GetProperty("application/json").GetProperty("schema").GetProperty("$ref").GetString());
            Assert.True(responses.TryGetProperty("404", out _));
            Assert.True(responses.TryGetProperty("409", out _));
        }
        var metricsPath = Environment.GetEnvironmentVariable("HOTEL_ERP_OPERATIONS_PAGINATION_METRICS");
        if (!string.IsNullOrWhiteSpace(metricsPath))
            await File.WriteAllTextAsync(metricsPath, JsonSerializer.Serialize(new { syntheticDatabase = true, lists = metrics },
                new JsonSerializerOptions { WriteIndented = true }));
    }
}
