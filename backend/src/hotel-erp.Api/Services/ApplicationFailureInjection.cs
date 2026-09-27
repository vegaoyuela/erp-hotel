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
}
