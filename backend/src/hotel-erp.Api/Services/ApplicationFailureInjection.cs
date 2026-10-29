namespace hotel_erp.Api.Services;

public interface IApplicationFailureInjector
{
    Task CheckpointAsync(string checkpoint, CancellationToken cancellationToken = default);
}

public sealed class NoOpApplicationFailureInjector : IApplicationFailureInjector
{
    public Task CheckpointAsync(string checkpoint, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}

public static class ApplicationFailureCheckpoints
{
    public const string SecurityAfterLock = "security:after-lock";
    public const string SecurityAfterPersistence = "security:after-persistence";
    public const string SecurityAfterRevocation = "security:after-revocation";
    public const string SecurityAfterAudit = "security:after-audit";
    public const string InvoiceAfterPersistence = "invoice:create:after-persistence";
    public const string InvoiceAfterAccounting = "invoice:create:after-accounting";
    public const string InvoiceAfterAudit = "invoice:create:after-audit";
    public const string InvoiceAfterIdempotency = "invoice:create:after-idempotency";
    public const string CreditNoteAfterPersistence = "invoice:credit-note:after-persistence";
    public const string CreditNoteAfterAccounting = "invoice:credit-note:after-accounting";
    public const string CreditNoteAfterAudit = "invoice:credit-note:after-audit";
    public const string CreditNoteAfterIdempotency = "invoice:credit-note:after-idempotency";
    public const string DebitNoteAfterPersistence = "invoice:debit-note:after-persistence";
    public const string DebitNoteAfterAccounting = "invoice:debit-note:after-accounting";
    public const string DebitNoteAfterAudit = "invoice:debit-note:after-audit";
    public const string DebitNoteAfterIdempotency = "invoice:debit-note:after-idempotency";
    public const string ReservationCreateAfterPersistence = "reservation:create:after-persistence";
    public const string ReservationUpdateAfterPersistence = "reservation:update:after-persistence";
    public const string ReservationConfirmAfterPersistence = "reservation:confirm:after-persistence";
    public const string ReservationCancelAfterPersistence = "reservation:cancel:after-persistence";
    public const string ReservationCheckInAfterPersistence = "reservation:check-in:after-persistence";
    public const string ReservationCheckOutAfterPersistence = "reservation:check-out:after-persistence";
    public const string FolioChargeAfterPersistence = "folio:charge:after-persistence";
    public const string CashRegisterCreateAfterPersistence = "cash-register:create:after-persistence";
    public const string CashRegisterOpenAfterPersistence = "cash-register:open:after-persistence";
    public const string CashRegisterCloseAfterPersistence = "cash-register:close:after-persistence";
    public const string PurchaseCreateAfterPersistence = "purchase:create:after-persistence";
    public const string PurchaseCreateAfterAccounting = "purchase:create:after-accounting";
    public const string PurchasePayAfterPersistence = "purchase:pay:after-persistence";
    public const string PurchaseDeleteAfterPersistence = "purchase:delete:after-persistence";
    public const string InventoryMovementAfterPersistence = "inventory:movement:after-persistence";
    public const string PaymentAfterAccounting = "payment:create:after-accounting";
    public const string RefundAfterAccounting = "refund:create:after-accounting";
    public const string CardSettlementAfterAccounting = "card-settlement:create:after-accounting";
}
