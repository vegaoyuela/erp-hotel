namespace hotel_erp.Api.Services.Interfaces;

public interface ITaxCalculator
{
    decimal IsvRate { get; set; }
    decimal TouristTaxRate { get; set; }
    decimal TaxFactor { get; }
    TaxResult CalculateFromNetAmount(decimal netAmount, bool isIsvExempt = false,
        bool isTouristTaxExempt = false, decimal discountPercentage = 0);
    TaxResult CalculateFromFinalPrice(decimal finalPrice, bool isIsvExempt = false,
        bool isTouristTaxExempt = false, decimal discountPercentage = 0);
    TaxResult CalculateFromSellingPrice(decimal sellingPricePerNight, int nights);
    TaxResult ApplyDiscount(TaxResult result, decimal discountPercentage);
}
