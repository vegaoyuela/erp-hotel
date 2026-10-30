using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record PurchaseOperationResult(Guid Id);

public sealed class PurchaseInvoiceApplicationService(
    ApplicationDbContext context,
    IAccountingService accountingService,
    IAuditService auditService,
    IApplicationFailureInjector failureInjector,
    OperationalMutationGuard access)
{
    public async Task<ApplicationMutationResult<PurchaseInvoice>> CreateAsync(
        SecurityActor actor,
        CreatePurchaseInvoiceRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = actor.Id;
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);

        var validation = ValidateItems(request.Items);
        if (validation is not null)
            return ApplicationMutationResult<PurchaseInvoice>.BadRequest(validation);

        var invoiceNumber = request.InvoiceNumber.Trim();
        var lockName = $"purchase-invoice:{request.SupplierId:N}:{invoiceNumber.ToUpperInvariant()}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({lockName}));",
            cancellationToken);

        var supplier = await context.Suppliers
            .SingleOrDefaultAsync(candidate => candidate.Id == request.SupplierId, cancellationToken);
        if (supplier is null || !supplier.IsActive)
            return ApplicationMutationResult<PurchaseInvoice>.BadRequest("Proveedor no encontrado o inactivo");

        if (await context.PurchaseInvoices.AnyAsync(
                candidate => candidate.SupplierId == request.SupplierId
                    && candidate.InvoiceNumber.ToLower() == invoiceNumber.ToLower(),
                cancellationToken))
        {
            return ApplicationMutationResult<PurchaseInvoice>.Conflict(
                "Ya existe una factura de compra con ese número para el proveedor");
        }

        var subtotal = 0m;
        var isv15 = 0m;
        var isv18 = 0m;
        var items = request.Items.Select(item =>
        {
            var lineTotal = TaxService.RoundCurrency(item.Quantity * item.UnitPrice);
            var tax = item.IsExempt ? 0m : TaxService.RoundCurrency(lineTotal * item.ISVRate);
            subtotal += lineTotal;
            if (!item.IsExempt && item.ISVRate == 0.15m) isv15 += tax;
            if (!item.IsExempt && item.ISVRate == 0.18m) isv18 += tax;
            return new PurchaseInvoiceItem
            {
                Description = item.Description.Trim(),
                Quantity = item.Quantity,
                UnitPrice = TaxService.RoundCurrency(item.UnitPrice),
                LineTotal = lineTotal,
                IsExempt = item.IsExempt,
                ISVRate = item.IsExempt ? 0m : item.ISVRate
            };
        }).ToList();

        subtotal = TaxService.RoundCurrency(subtotal);
        isv15 = TaxService.RoundCurrency(isv15);
        isv18 = TaxService.RoundCurrency(isv18);
        var totalIsv = TaxService.RoundCurrency(isv15 + isv18);
        var purchaseInvoice = new PurchaseInvoice
        {
            InvoiceNumber = invoiceNumber,
            SupplierId = request.SupplierId,
            Supplier = supplier,
            InvoiceDate = request.InvoiceDate,
            CAINumber = NullIfWhiteSpace(request.CAINumber),
            SupplierRTN = NullIfWhiteSpace(request.SupplierRTN) ?? supplier.RTN,
            SubTotal = subtotal,
            ISVAmount = totalIsv,
            ISV15Amount = isv15,
            ISV18Amount = isv18,
            TotalAmount = TaxService.RoundCurrency(subtotal + totalIsv),
            Notes = NullIfWhiteSpace(request.Notes),
            Status = PurchaseInvoiceStatus.Pendiente,
            PurchaseInvoiceItems = items
        };

        context.PurchaseInvoices.Add(purchaseInvoice);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.PurchaseCreateAfterPersistence,
            cancellationToken);
        await accountingService.CreatePurchaseEntryAsync(purchaseInvoice);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.PurchaseCreateAfterAccounting,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreatePurchaseInvoice",
            nameof(PurchaseInvoice),
            purchaseInvoice.Id,
            new
            {
                purchaseInvoice.InvoiceNumber,
                purchaseInvoice.SupplierId,
                purchaseInvoice.SubTotal,
                purchaseInvoice.ISV15Amount,
                purchaseInvoice.ISV18Amount,
                purchaseInvoice.TotalAmount
            });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<PurchaseInvoice>.Success(purchaseInvoice);
    }

    public async Task<ApplicationMutationResult<PurchaseOperationResult>> MarkAsPaidAsync(
        Guid id,
        SecurityActor actor,
        CancellationToken cancellationToken = default)
    {
        var userId = actor.Id;
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);

        await LockPurchaseAsync(id, cancellationToken);
        var purchase = await context.PurchaseInvoices
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (purchase is null)
            return ApplicationMutationResult<PurchaseOperationResult>.NotFound("Factura de compra no encontrada");

        if (purchase.Status == PurchaseInvoiceStatus.Pagada)
        {
            await transaction.CommitAsync(cancellationToken);
            return ApplicationMutationResult<PurchaseOperationResult>.Success(new(id));
        }

        purchase.Status = PurchaseInvoiceStatus.Pagada;
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.PurchasePayAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "PayPurchaseInvoice",
            nameof(PurchaseInvoice),
            id,
            new { Status = purchase.Status.ToString() });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<PurchaseOperationResult>.Success(new(id));
    }

    public async Task<ApplicationMutationResult<PurchaseOperationResult>> DeleteAsync(
        Guid id,
        SecurityActor actor,
        CancellationToken cancellationToken = default)
    {
        var userId = actor.Id;
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);

        await LockPurchaseAsync(id, cancellationToken);
        var purchase = await context.PurchaseInvoices
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (purchase is null)
            return ApplicationMutationResult<PurchaseOperationResult>.NotFound("Factura de compra no encontrada");

        await accountingService.DeleteEntryByReferenceIdAsync(id);
        purchase.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.PurchaseDeleteAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "DeletePurchaseInvoice",
            nameof(PurchaseInvoice),
            id,
            new { purchase.InvoiceNumber, purchase.TotalAmount });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<PurchaseOperationResult>.Success(new(id));
    }

    private async Task LockPurchaseAsync(Guid id, CancellationToken cancellationToken)
    {
        var lockName = $"purchase-invoice:{id:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({lockName}));",
            cancellationToken);
    }

    private static string? ValidateItems(IEnumerable<PurchaseInvoiceItemDto> items)
    {
        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Description))
                return "Cada línea de compra requiere una descripción";
            if (item.Quantity <= 0)
                return "La cantidad de cada línea debe ser mayor que cero";
            if (item.UnitPrice < 0m)
                return "El precio unitario no puede ser negativo";
            if (!item.IsExempt && item.ISVRate is not 0.15m and not 0.18m)
                return "La tasa ISV gravada debe ser 15 % o 18 %";
        }

        return null;
    }

    private static string? NullIfWhiteSpace(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
