using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.IntegrationTests;

public class RepositoryUnitOfWorkTests
{
    [PostgreSqlFact]
    public async Task StagedChanges_RequireExplicitSave_AndRespectOuterRollback()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        await using var database = new TemporaryPostgreSqlDatabase(
            Environment.GetEnvironmentVariable("HOTEL_ERP_TEST_ADMIN_CONNECTION")!);
        await database.CreateAsync();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(database.ConnectionString).Options;
        await using var writer = new ApplicationDbContext(options);
        await writer.Database.MigrateAsync();
        var unitOfWork = new EfUnitOfWork(writer);
        var suppliers = new SupplierRepository(writer);
        var guests = new GuestRepository(writer);
        var supplier = new Supplier { Name = "Proveedor unidad de trabajo" };
        var guest = new Guest { FirstName = "Huésped", LastName = "Transaccional" };

        await suppliers.AddAsync(supplier);
        await guests.AddAsync(guest);
        await using (var observer = new ApplicationDbContext(options))
        {
            Assert.False(await observer.Suppliers.AnyAsync());
            Assert.False(await observer.Guests.AnyAsync());
        }

        await unitOfWork.SaveChangesAsync();
        await using (var observer = new ApplicationDbContext(options))
        {
            Assert.True(await observer.Suppliers.AnyAsync(candidate => candidate.Id == supplier.Id));
            Assert.True(await observer.Guests.AnyAsync(candidate => candidate.Id == guest.Id));
        }

        supplier.Name = "Cambio sin confirmar";
        await suppliers.UpdateAsync(supplier);
        await guests.DeleteAsync(guest.Id);
        await using (var transaction = await writer.Database.BeginTransactionAsync())
        {
            await unitOfWork.SaveChangesAsync();
            await using var observer = new ApplicationDbContext(options);
            Assert.Equal("Proveedor unidad de trabajo", (await observer.Suppliers.SingleAsync()).Name);
            Assert.True(await observer.Guests.AnyAsync(candidate => candidate.Id == guest.Id));
            await transaction.RollbackAsync();
        }

        writer.ChangeTracker.Clear();
        await using (var observer = new ApplicationDbContext(options))
        {
            Assert.Equal("Proveedor unidad de trabajo", (await observer.Suppliers.SingleAsync()).Name);
            Assert.False((await observer.Guests.SingleAsync()).IsDeleted);
        }

        // A failing statement must roll back every staged entity in the flush.
        await suppliers.AddAsync(new Supplier { Name = "No debe sobrevivir" });
        var accounts = new AccountingRepository(writer);
        await accounts.AddAccountAsync(new AccountingAccount
        {
            AccountNumber = "9901",
            AccountName = "Cuenta duplicada A",
            AccountType = AccountType.Activo
        });
        await accounts.AddAccountAsync(new AccountingAccount
        {
            AccountNumber = "9901",
            AccountName = "Cuenta duplicada B",
            AccountType = AccountType.Activo
        });
        await Assert.ThrowsAsync<DbUpdateException>(() => unitOfWork.SaveChangesAsync());
        await using (var observer = new ApplicationDbContext(options))
        {
            Assert.Single(await observer.Suppliers.ToListAsync());
            Assert.Empty(await observer.AccountingAccounts.ToListAsync());
        }
    }
}
