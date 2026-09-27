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
}
