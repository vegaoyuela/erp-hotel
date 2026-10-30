using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
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
    private static readonly Type[] CatalogServices = [typeof(CustomerApplicationService), typeof(GuestApplicationService),
        typeof(RoomApplicationService), typeof(RoomTypeApplicationService), typeof(DiscountApplicationService), typeof(SupplierApplicationService),
        typeof(CaiApplicationService), typeof(DocumentAuthorizationApplicationService), typeof(TaxConfigurationApplicationService),
        typeof(BusinessSettingsApplicationService), typeof(AccountingCatalogApplicationService)];
    private static readonly string[] CatalogPermissions = [PermissionNames.ManageReservations, PermissionNames.ManageSettings,
        PermissionNames.ManageTaxes, PermissionNames.ManageAccounting];
    private const string CatalogPrivateMarker = "PRIVATE_CATALOG_QA_VALUE";
    private sealed record CatalogCase(string Name, HttpMethod Method, string Path, object? Body, HttpStatusCode Success, bool WithFile = false);
    private sealed class CatalogTestInjector : IApplicationFailureInjector
    {
        public SecurityTestInjector Security { get; } = new();
        private string? _holdAt;
        private TaskCompletionSource _entered = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private TaskCompletionSource _release = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public Task Entered => _entered.Task;
        public void HoldNext(string checkpoint)
        {
            _entered = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _release = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _holdAt = checkpoint;
        }
        public void Release() => _release.TrySetResult();
        public async Task CheckpointAsync(string checkpoint, CancellationToken cancellationToken = default)
        {
            await Security.CheckpointAsync(checkpoint, cancellationToken);
            if (checkpoint == _holdAt && Interlocked.CompareExchange(ref _holdAt, null, checkpoint) == checkpoint)
            {
                _entered.TrySetResult();
                await _release.Task.WaitAsync(TimeSpan.FromSeconds(20), cancellationToken);
            }
        }
    }
    private static HotelErpWebApplicationFactory CatalogFactory(string connection, CatalogTestInjector injector, string files)
        => new(connection, services =>
        {
            services.RemoveAll<IApplicationFailureInjector>();
            services.AddSingleton<IApplicationFailureInjector>(injector);
            services.RemoveAll<AuthorizationAttachmentOptions>();
            services.AddSingleton(new AuthorizationAttachmentOptions { Directory = files });
        });

    private static async Task GrantCatalogPermissionsAsync(IServiceProvider services, SecurityTestIds ids)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var existing = await db.RolePermissions.Where(link => link.RoleId == ids.WorkerRole).Select(link => link.PermissionId).ToListAsync();
        foreach (var permission in await db.Permissions.Where(row => CatalogPermissions.Contains(row.Name)).ToListAsync())
            if (!existing.Contains(permission.Id)) db.RolePermissions.Add(new RolePermission { RoleId = ids.WorkerRole, PermissionId = permission.Id });
        await db.SaveChangesAsync();
    }

    private static async Task<IReadOnlyList<CatalogCase>> PrepareCatalogCasesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var today = scope.ServiceProvider.GetRequiredService<IHotelClock>().Today;
        var ids = Enumerable.Range(0, 26).Select(_ => Guid.NewGuid()).ToArray();
        db.Customers.AddRange(new Customer { Id = ids[0], Name = "Customer update QA" }, new Customer { Id = ids[1], Name = "Customer delete QA" });
        db.Guests.AddRange(new Guest { Id = ids[2], FirstName = "Guest", LastName = "Update" },
            new Guest { Id = ids[3], FirstName = "Guest", LastName = "Classify" }, new Guest { Id = ids[4], FirstName = "Guest", LastName = "Delete" });
        db.RoomTypes.AddRange(new RoomType { Id = ids[5], Name = "Base room QA", PricePerNight = 100, Capacity = 2 },
            new RoomType { Id = ids[6], Name = "Update room QA", PricePerNight = 100, Capacity = 2 },
            new RoomType { Id = ids[7], Name = "Delete room QA", PricePerNight = 100, Capacity = 2 });
        db.Rooms.AddRange(new Room { Id = ids[8], RoomNumber = "Q-U", RoomTypeId = ids[5] }, new Room { Id = ids[9], RoomNumber = "Q-D", RoomTypeId = ids[5] });
        db.Discounts.AddRange(new Discount { Id = ids[10], Name = "Update discount QA", Value = 1 }, new Discount { Id = ids[11], Name = "Delete discount QA", Value = 1 });
        db.Suppliers.AddRange(new Supplier { Id = ids[12], Name = "Update supplier QA" }, new Supplier { Id = ids[13], Name = "Delete supplier QA" });
        db.CAIs.Add(new CAI
        {
            Id = ids[14],
            CAINumber = "QA-INACTIVE-CAI-DELETE",
            Status = CAIStatus.Desactivado,
            IssueDate = today.AddDays(-1),
            DueDate = today.AddYears(1),
            InitialRange = "001-001-01-00000100",
            FinalRange = "001-001-01-00000199",
            CurrentCorrelative = "001-001-01-00000100"
        });
        db.DocumentAuthorizations.Add(new DocumentAuthorization
        {
            Id = ids[15],
            CAINumber = "QA-DELETE-AUTHORIZATION",
            DocumentType = InvoiceDocumentType.NotaCredito,
            Status = CAIStatus.Desactivado,
            IssueDate = today.AddDays(-1),
            DueDate = today.AddYears(1),
            InitialRange = "001-001-07-00000100",
            FinalRange = "001-001-07-00000199",
            CurrentCorrelative = "001-001-07-00000100"
        });
        db.TaxConfigurations.AddRange(new TaxConfiguration { Id = ids[16], Name = "Update tax QA", Rate = .15m }, new TaxConfiguration { Id = ids[17], Name = "Delete tax QA", Rate = .15m });
        db.AccountingAccounts.AddRange(new AccountingAccount { Id = ids[18], AccountNumber = "QA-U", AccountName = "Update QA", AccountType = AccountType.Activo },
            new AccountingAccount { Id = ids[19], AccountNumber = "QA-D", AccountName = "Delete QA", AccountType = AccountType.Activo },
            new AccountingAccount { Id = ids[20], AccountNumber = "QA-DR", AccountName = "Debit QA", AccountType = AccountType.Activo },
            new AccountingAccount { Id = ids[21], AccountNumber = "QA-CR", AccountName = "Credit QA", AccountType = AccountType.Pasivo },
            new AccountingAccount { Id = ids[22], AccountNumber = "QA-H", AccountName = "History QA", AccountType = AccountType.Activo });
        db.AccountingEntries.Add(new AccountingEntry
        {
            Id = ids[23],
            TransactionDate = today,
            Description = "Historical fixture QA",
            EntryItems = [new EntryItem { AccountId = ids[22], Debit = 1 }, new EntryItem { AccountId = ids[21], Credit = 1 }]
        });
        var settings = await db.BusinessSettings.SingleOrDefaultAsync();
        if (settings is null) { settings = new BusinessSettings(); db.BusinessSettings.Add(settings); }
        settings.BusinessName = "Catalog hotel QA"; settings.RTN = "08019000123456"; settings.Address = "Synthetic QA address";
        settings.FiscalProfileStatus = FiscalProfileStatus.Borrador;
        await db.SaveChangesAsync();
        object Authorization(string type, string number, string code) => new
        {
            documentType = type,
            caiNumber = number,
            issueDate = today.AddDays(-1),
            dueDate = today.AddYears(1),
            initialRange = $"001-001-{code}-00000001",
            finalRange = $"001-001-{code}-00000099"
        };
        return [
            new("Customers.Create", HttpMethod.Post, "/api/customers", new { name = CatalogPrivateMarker }, HttpStatusCode.Created),
            new("Customers.Update", HttpMethod.Put, $"/api/customers/{ids[0]}", new { name = CatalogPrivateMarker }, HttpStatusCode.NoContent),
            new("Customers.Delete", HttpMethod.Delete, $"/api/customers/{ids[1]}", null, HttpStatusCode.NoContent),
            new("Guests.Create", HttpMethod.Post, "/api/guests", new { firstName = CatalogPrivateMarker, lastName = "Synthetic" }, HttpStatusCode.Created),
            new("Guests.Update", HttpMethod.Put, $"/api/guests/{ids[2]}", new { firstName = CatalogPrivateMarker }, HttpStatusCode.NoContent),
            new("Guests.UpdateClassification", HttpMethod.Patch, $"/api/guests/{ids[3]}/classification", new { classification = "Frequent QA" }, HttpStatusCode.OK),
            new("Guests.Delete", HttpMethod.Delete, $"/api/guests/{ids[4]}", null, HttpStatusCode.NoContent),
            new("Rooms.Create", HttpMethod.Post, "/api/rooms", new { roomNumber = "Q-NEW", floor = 1, roomTypeId = ids[5] }, HttpStatusCode.Created),
            new("Rooms.Update", HttpMethod.Put, $"/api/rooms/{ids[8]}", new { observations = CatalogPrivateMarker }, HttpStatusCode.NoContent),
            new("Rooms.Delete", HttpMethod.Delete, $"/api/rooms/{ids[9]}", null, HttpStatusCode.NoContent),
            new("RoomTypes.Create", HttpMethod.Post, "/api/room-types", new { name = "New room QA", pricePerNight = 100, capacity = 2 }, HttpStatusCode.Created),
            new("RoomTypes.Update", HttpMethod.Put, $"/api/room-types/{ids[6]}", new { description = CatalogPrivateMarker }, HttpStatusCode.NoContent),
            new("RoomTypes.Delete", HttpMethod.Delete, $"/api/room-types/{ids[7]}", null, HttpStatusCode.NoContent),
            new("Discounts.Create", HttpMethod.Post, "/api/discounts", new { name = "New discount QA", value = 1 }, HttpStatusCode.Created),
            new("Discounts.Update", HttpMethod.Put, $"/api/discounts/{ids[10]}", new { description = CatalogPrivateMarker }, HttpStatusCode.NoContent),
            new("Discounts.Delete", HttpMethod.Delete, $"/api/discounts/{ids[11]}", null, HttpStatusCode.NoContent),
            new("Suppliers.Create", HttpMethod.Post, "/api/suppliers", new { name = CatalogPrivateMarker }, HttpStatusCode.Created),
            new("Suppliers.Update", HttpMethod.Put, $"/api/suppliers/{ids[12]}", new { name = CatalogPrivateMarker }, HttpStatusCode.NoContent),
            new("Suppliers.Delete", HttpMethod.Delete, $"/api/suppliers/{ids[13]}", null, HttpStatusCode.NoContent),
            new("CAI.Create", HttpMethod.Post, "/api/cai", Authorization("Factura", "QA-NEW-CAI-NUMBER", "01"), HttpStatusCode.Created),
            new("CAI.Delete", HttpMethod.Delete, $"/api/cai/{ids[14]}", null, HttpStatusCode.NoContent),
            new("DocumentAuthorizations.Create", HttpMethod.Post, "/api/document-authorizations", Authorization("Factura", "QA-NEW-AUTHORIZATION", "01"), HttpStatusCode.Created),
            new("DocumentAuthorizations.Delete", HttpMethod.Delete, $"/api/document-authorizations/{ids[15]}", null, HttpStatusCode.NoContent),
            new("DocumentAuthorizations.CreateWithFile", HttpMethod.Post, "/api/document-authorizations/with-file", Authorization("NotaDebito", "QA-NEW-FILE-AUTHORIZATION", "08"), HttpStatusCode.Created, true),
            new("TaxConfigurations.Create", HttpMethod.Post, "/api/tax-configurations", new { name = "New tax QA", rate = .15m, isActive = true }, HttpStatusCode.Created),
            new("TaxConfigurations.Update", HttpMethod.Put, $"/api/tax-configurations/{ids[16]}", new { name = "Updated tax QA", rate = .18m, isActive = true }, HttpStatusCode.NoContent),
            new("TaxConfigurations.Delete", HttpMethod.Delete, $"/api/tax-configurations/{ids[17]}", null, HttpStatusCode.NoContent),
            new("Settings.Update", HttpMethod.Put, "/api/settings/business", new { footer = CatalogPrivateMarker }, HttpStatusCode.OK),
            new("Settings.ApproveFiscalProfile", HttpMethod.Post, "/api/settings/business/approve", new { validFrom = today.AddDays(-1), validUntil = today.AddYears(1), approvalNote = "Synthetic QA approval only", decisionManifestSha256 = new string('a',64), evidenceReference = "qa://catalog-profile" }, HttpStatusCode.OK),
            new("Settings.RetireFiscalProfile", HttpMethod.Post, "/api/settings/business/retire", new { reason = "Synthetic QA retirement only" }, HttpStatusCode.OK),
            new("Accounting.Create", HttpMethod.Post, "/api/accounts", new { accountNumber = "QA-NEW", accountName = "New account QA", accountType = "Activo" }, HttpStatusCode.Created),
            new("Accounting.Update", HttpMethod.Put, $"/api/accounts/{ids[18]}", new { accountName = "Updated account QA" }, HttpStatusCode.NoContent),
            new("Accounting.Deactivate", HttpMethod.Delete, $"/api/accounts/{ids[19]}", null, HttpStatusCode.OK),
            new("Accounting.CreateJournalEntry", HttpMethod.Post, "/api/accounts/journal-entries", new { transactionDate = today, description = CatalogPrivateMarker, entryType = "Diario", items = new[] { new { accountId = ids[20], debit = 5, credit = 0 }, new { accountId = ids[21], debit = 0, credit = 5 } } }, HttpStatusCode.Created),
            new("Accounting.DeactivateWithHistory", HttpMethod.Delete, $"/api/accounts/{ids[22]}", null, HttpStatusCode.OK)
        ];
    }

    private static async Task<HttpResponseMessage> SendCatalogCaseAsync(HttpClient client, string? token, CatalogCase item, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(item.Method, item.Path);
        request.Headers.Authorization = new("Bearer", token);
        if (item.WithFile)
        {
            var content = new MultipartFormDataContent();
            foreach (var property in JsonSerializer.SerializeToElement(item.Body).EnumerateObject())
                content.Add(new StringContent(property.Value.ToString()), property.Name);
            var pdf = new ByteArrayContent("%PDF-1.4\n% Synthetic catalog QA\n%%EOF"u8.ToArray());
            pdf.Headers.ContentType = new("application/pdf");
            content.Add(pdf, "file", "catalog-private.pdf");
            request.Content = content;
        }
        else if (item.Body is not null) request.Content = JsonContent.Create(item.Body);
        return await client.SendAsync(request, cancellationToken);
    }

    private static async Task<string> CatalogStateDigestAsync(string connection, bool includeAudit)
    {
        await using var db = new NpgsqlConnection(connection);
        await db.OpenAsync();
        var tables = new List<string>();
        await using (var command = db.CreateCommand())
        {
            command.CommandText = "SELECT tablename FROM pg_tables WHERE schemaname='public' ORDER BY tablename";
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) tables.Add(reader.GetString(0));
        }
        var excluded = new[] { "Users", "Roles", "Permissions", "UserRoles", "RolePermissions", "RefreshTokens", "__EFMigrationsHistory" };
        var state = new StringBuilder();
        foreach (var table in tables.Where(table => !excluded.Contains(table) && (includeAudit || table is not ("AuditLogs" or "CorrelativeLocks"))))
        {
            await using var command = db.CreateCommand();
            command.CommandText = $"SELECT COALESCE(jsonb_agg(to_jsonb(t) ORDER BY to_jsonb(t)::text),'[]'::jsonb)::text FROM {new NpgsqlCommandBuilder().QuoteIdentifier(table)} t";
            state.Append(table).Append(await command.ExecuteScalarAsync());
        }
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(state.ToString())));
    }
    private static string CatalogFilesDigest(string directory)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("|", Directory.Exists(directory)
            ? Directory.GetFiles(directory, "*", SearchOption.AllDirectories).Order().Select(file => Path.GetRelativePath(directory, file) + ":" + Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(file)))) : []))));
}
