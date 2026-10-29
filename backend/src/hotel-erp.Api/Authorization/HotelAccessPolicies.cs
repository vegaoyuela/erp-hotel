namespace hotel_erp.Api.Authorization;

// Read policies reuse the operator's effective permissions; role labels never grant access.
public static class HotelAccessPolicies
{
    public const string ReadGuests = "read_guest_data";
    public const string ReadFolios = "read_folios";
    public const string ReadInvoices = "read_invoices";
    public const string ReadFiscalOptions = "read_fiscal_options";
    public const string ReadDiscounts = "read_discounts";
    public const string ReadTaxRates = "read_tax_rates";
    public const string ReadSettings = "read_business_settings";
    public const string ReadPayments = "read_payments";

    public static readonly IReadOnlyDictionary<string, string[]> AnyOf = new Dictionary<string, string[]>
    {
        [ReadGuests] = [PermissionNames.ManageReservations, PermissionNames.ManageAccounting],
        [ReadFolios] = [PermissionNames.ManageReservations, PermissionNames.CreateInvoices, PermissionNames.ManageCash, PermissionNames.ManageAccounting],
        [ReadInvoices] = [PermissionNames.CreateInvoices, PermissionNames.AdjustInvoices, PermissionNames.ManageCash, PermissionNames.ManageAccounting, PermissionNames.ManageTaxes],
        [ReadFiscalOptions] = [PermissionNames.CreateInvoices, PermissionNames.AdjustInvoices, PermissionNames.ManageTaxes],
        [ReadDiscounts] = [PermissionNames.ManageSettings, PermissionNames.ManageReservations, PermissionNames.CreateInvoices],
        [ReadTaxRates] = [PermissionNames.ManageTaxes, PermissionNames.CreateInvoices, PermissionNames.ManageReservations],
        [ReadSettings] = [PermissionNames.ManageSettings, PermissionNames.ManageTaxes],
        [ReadPayments] = [PermissionNames.ManageCash, PermissionNames.ManageAccounting]
    };
}
