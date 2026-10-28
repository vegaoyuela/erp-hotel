namespace hotel_erp.Api.Services.Interfaces;

public interface IInvoiceCalculator
{
    InvoiceCalculation Calculate(
        IEnumerable<InvoiceLineInput> source,
        bool isIsvExempt = false,
        bool isTouristTaxExempt = false);
}
