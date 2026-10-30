using hotel_erp.Api.Controllers;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;

namespace hotel_erp.UnitTests;

public class ApplicationServiceArchitectureTests
{
    [Theory]
    [InlineData(typeof(CustomersController), typeof(CustomerApplicationService))]
    [InlineData(typeof(GuestsController), typeof(GuestApplicationService))]
    [InlineData(typeof(RoomsController), typeof(RoomApplicationService))]
    [InlineData(typeof(RoomTypesController), typeof(RoomTypeApplicationService))]
    [InlineData(typeof(DiscountsController), typeof(DiscountApplicationService))]
    [InlineData(typeof(SuppliersController), typeof(SupplierApplicationService))]
    [InlineData(typeof(CAIController), typeof(CaiApplicationService))]
    [InlineData(typeof(DocumentAuthorizationsController), typeof(DocumentAuthorizationApplicationService))]
    [InlineData(typeof(TaxConfigurationsController), typeof(TaxConfigurationApplicationService))]
    [InlineData(typeof(SettingsController), typeof(BusinessSettingsApplicationService))]
    [InlineData(typeof(AccountsController), typeof(AccountingCatalogApplicationService))]
    public void CatalogMutationController_DelegatesPersistenceAndAudit(Type controller, Type application)
    {
        var dependencies = Assert.Single(controller.GetConstructors()).GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        Assert.Contains(application, dependencies);
        Assert.DoesNotContain(typeof(IUnitOfWork), dependencies);
        Assert.DoesNotContain(typeof(IAuditService), dependencies);
    }


    [Theory]
    [InlineData(typeof(InvoiceApplicationService))]
    [InlineData(typeof(PaymentApplicationService))]
    [InlineData(typeof(RefundApplicationService))]
    [InlineData(typeof(CardSettlementApplicationService))]
    [InlineData(typeof(ReservationApplicationService))]
    [InlineData(typeof(FolioApplicationService))]
    [InlineData(typeof(CashRegisterApplicationService))]
    [InlineData(typeof(PurchaseInvoiceApplicationService))]
    [InlineData(typeof(InventoryApplicationService))]
    [InlineData(typeof(CustomerApplicationService))]
    [InlineData(typeof(GuestApplicationService))]
    [InlineData(typeof(RoomApplicationService))]
    [InlineData(typeof(RoomTypeApplicationService))]
    [InlineData(typeof(DiscountApplicationService))]
    [InlineData(typeof(SupplierApplicationService))]
    [InlineData(typeof(CaiApplicationService))]
    [InlineData(typeof(DocumentAuthorizationApplicationService))]
    [InlineData(typeof(TaxConfigurationApplicationService))]
    [InlineData(typeof(BusinessSettingsApplicationService))]
    [InlineData(typeof(AccountingCatalogApplicationService))]
    public void OperationalMutation_RequiresVersionedActorAndTransactionGuard(Type service)
    {
        Assert.Contains(Assert.Single(service.GetConstructors()).GetParameters(), parameter => parameter.ParameterType == typeof(OperationalMutationGuard));
        var methods = service.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)
            .Where(method => typeof(Task).IsAssignableFrom(method.ReturnType));
        Assert.All(methods, method => Assert.Contains(method.GetParameters(), parameter => parameter.ParameterType == typeof(SecurityActor)));
    }

    [Theory]
    [InlineData(typeof(UsersController))]
    [InlineData(typeof(RolesController))]
    public void SecurityMutationController_DelegatesToTransactionalService(Type controller)
    {
        var dependencies = Assert.Single(controller.GetConstructors()).GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        Assert.Contains(typeof(SecurityAdministrationService), dependencies);
        Assert.DoesNotContain(typeof(hotel_erp.Api.Database.ApplicationDbContext), dependencies);
        Assert.DoesNotContain(typeof(IAuditService), dependencies);
        Assert.DoesNotContain(typeof(IJwtService), dependencies);
        Assert.DoesNotContain(typeof(IUnitOfWork), dependencies);
    }

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
        Assert.DoesNotContain(typeof(IAuditService), dependencies);
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
        Assert.DoesNotContain(typeof(IAuditService), dependencies);
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
        Assert.DoesNotContain(typeof(IAuditService), dependencies);
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

    [Theory]
    [InlineData(typeof(InvoiceApplicationService), typeof(IInvoiceCalculator))]
    [InlineData(typeof(FolioApplicationService), typeof(IInvoiceCalculator))]
    [InlineData(typeof(ReservationApplicationService), typeof(ITaxCalculator))]
    [InlineData(typeof(InvoiceApplicationService), typeof(IAuditService))]
    [InlineData(typeof(FolioApplicationService), typeof(IAuditService))]
    [InlineData(typeof(ReservationApplicationService), typeof(IAuditService))]
    [InlineData(typeof(PaymentApplicationService), typeof(IAuditService))]
    [InlineData(typeof(RefundApplicationService), typeof(IAuditService))]
    [InlineData(typeof(CardSettlementApplicationService), typeof(IAuditService))]
    [InlineData(typeof(CashRegisterApplicationService), typeof(IAuditService))]
    [InlineData(typeof(PurchaseInvoiceApplicationService), typeof(IAuditService))]
    [InlineData(typeof(InventoryApplicationService), typeof(IAuditService))]
    [InlineData(typeof(AuthService), typeof(IAuditService))]
    [InlineData(typeof(BusinessSettingsApplicationService), typeof(IAuditService))]
    [InlineData(typeof(CatalogMutationCommitter), typeof(IAuditService))]
    [InlineData(typeof(PrintController), typeof(IInvoicePrintRenderer))]
    [InlineData(typeof(PrintController), typeof(IPrinterTransport))]
    public void Consumer_RequiresReplaceableContract(Type consumer, Type contract)
    {
        var dependencies = Assert.Single(consumer.GetConstructors())
            .GetParameters().Select(parameter => parameter.ParameterType).ToHashSet();
        Assert.Contains(contract, dependencies);
        Assert.DoesNotContain(typeof(AuditService), dependencies);
        Assert.DoesNotContain(typeof(TaxService), dependencies);
        Assert.DoesNotContain(typeof(InvoiceCalculationService), dependencies);
        Assert.DoesNotContain(typeof(EscPosService), dependencies);
    }
}
