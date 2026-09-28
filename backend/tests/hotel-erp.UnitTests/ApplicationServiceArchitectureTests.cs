using hotel_erp.Api.Controllers;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;

namespace hotel_erp.UnitTests;

public class ApplicationServiceArchitectureTests
{
    [Theory]
    [InlineData(typeof(PaymentsController), typeof(PaymentApplicationService))]
    [InlineData(typeof(RefundsController), typeof(RefundApplicationService))]
    [InlineData(typeof(CardSettlementsController), typeof(CardSettlementApplicationService))]
    [InlineData(typeof(InvoicesController), typeof(InvoiceApplicationService))]
    public void FinancialMutationController_DelegatesToExpectedApplicationService(
        Type controllerType,
        Type applicationServiceType)
    {
        var constructor = Assert.Single(controllerType.GetConstructors());
        var dependencies = constructor.GetParameters().Select(parameter => parameter.ParameterType).ToHashSet();

        Assert.Contains(applicationServiceType, dependencies);
        Assert.DoesNotContain(typeof(IAccountingService), dependencies);
        Assert.DoesNotContain(typeof(AuditService), dependencies);
        Assert.DoesNotContain(typeof(IdempotencyService), dependencies);
        Assert.DoesNotContain(typeof(IHotelClock), dependencies);
    }

    [Theory]
    [InlineData(typeof(ReservationsController), typeof(ReservationApplicationService))]
    [InlineData(typeof(FoliosController), typeof(FolioApplicationService))]
    [InlineData(typeof(CashRegistersController), typeof(CashRegisterApplicationService))]
    public void OperationalMutationController_DelegatesToExpectedApplicationService(
        Type controllerType,
        Type applicationServiceType)
    {
        var constructor = Assert.Single(controllerType.GetConstructors());
        var dependencies = constructor.GetParameters().Select(parameter => parameter.ParameterType).ToHashSet();

        Assert.Contains(applicationServiceType, dependencies);
        Assert.DoesNotContain(typeof(AuditService), dependencies);
        Assert.DoesNotContain(typeof(IdempotencyService), dependencies);
    }

    [Theory]
    [InlineData(typeof(PurchaseInvoicesController), typeof(PurchaseInvoiceApplicationService))]
    [InlineData(typeof(InventoryController), typeof(InventoryApplicationService))]
    public void StockMutationController_DelegatesToExpectedApplicationService(
        Type controllerType,
        Type applicationServiceType)
    {
        var constructor = Assert.Single(controllerType.GetConstructors());
        var dependencies = constructor.GetParameters().Select(parameter => parameter.ParameterType).ToHashSet();

        Assert.Contains(applicationServiceType, dependencies);
        Assert.DoesNotContain(typeof(IAccountingService), dependencies);
        Assert.DoesNotContain(typeof(AuditService), dependencies);
        Assert.DoesNotContain(typeof(IHotelClock), dependencies);
    }

    [Fact]
    public void BackupController_DependsOnBackupContract()
    {
        var constructor = Assert.Single(typeof(BackupController).GetConstructors());
        var dependencies = constructor.GetParameters().Select(parameter => parameter.ParameterType).ToHashSet();

        Assert.Contains(typeof(IDatabaseBackupService), dependencies);
        Assert.DoesNotContain(typeof(DatabaseBackupService), dependencies);
    }
}
