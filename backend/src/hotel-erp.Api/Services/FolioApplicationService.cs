using hotel_erp.Api.Authorization;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record FolioChargeResult(string Message, Guid ItemId, decimal TotalAmount);

public sealed class FolioApplicationService(
    ApplicationDbContext context,
    IAuditService auditService,
    IInvoiceCalculator invoiceCalculator,
    IApplicationFailureInjector failureInjector,
    OperationalMutationGuard access)
{
    public async Task<ApplicationMutationResult<FolioChargeResult>> AddItemAsync(
        Guid folioId,
        SecurityActor actor,
        AddFolioItemRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = actor.Id;
        await using var transaction = await access.BeginAsync(actor, PermissionNames.CreateInvoices, cancellationToken);

        var folioLock = $"folio-settlement:{folioId:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({folioLock}));",
            cancellationToken);

        var folio = await context.Folios
            .Include(candidate => candidate.FolioItems)
            .SingleOrDefaultAsync(candidate => candidate.Id == folioId, cancellationToken);
        if (folio is null)
            return ApplicationMutationResult<FolioChargeResult>.NotFound("Folio no encontrado");
        if (folio.Status != FolioStatus.Abierto)
            return ApplicationMutationResult<FolioChargeResult>.Conflict("El folio no está abierto para recibir cargos");

        InvoiceCalculation calculation;
        try
        {
            calculation = invoiceCalculator.Calculate(
                folio.FolioItems.Select(item => new InvoiceLineInput(
                        item.Description,
                        item.Quantity,
                        item.UnitPrice,
                        item.IsExempt,
                        item.ISVRate,
                        item.IsTouristTaxable,
                        item.DiscountPercentage))
                    .Append(new InvoiceLineInput(
                        request.Description,
                        request.Quantity,
                        request.UnitPrice,
                        request.IsExempt,
                        request.ISVRate,
                        request.IsTouristTaxable,
                        request.DiscountPercentage)));
        }
        catch (InvoiceCalculationException exception)
        {
            return ApplicationMutationResult<FolioChargeResult>.BadRequest(exception.Message);
        }

        var item = new FolioItem
        {
            FolioId = folioId,
            Description = request.Description.Trim(),
            Quantity = request.Quantity,
            UnitPrice = TaxService.RoundCurrency(request.UnitPrice),
            LineTotal = TaxService.RoundCurrency(request.Quantity * request.UnitPrice),
            IsExempt = request.IsExempt,
            ISVRate = request.ISVRate,
            IsTouristTaxable = request.IsTouristTaxable,
            DiscountPercentage = request.DiscountPercentage
        };
        folio.FolioItems.Add(item);
        folio.TotalAmount = calculation.TotalAmount;

        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.FolioChargeAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "AddFolioCharge",
            nameof(Folio),
            folio.Id,
            new { ItemId = item.Id, item.Description, item.Quantity, item.UnitPrice, folio.TotalAmount });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<FolioChargeResult>.Success(
            new("Item agregado al folio", item.Id, folio.TotalAmount));
    }
}
