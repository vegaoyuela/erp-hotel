using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed class TaxConfigurationApplicationService(
    ApplicationDbContext _context, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateTaxConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("TaxConfigurations", cancellationToken);

        var existing = await _context.TaxConfigurations.FirstOrDefaultAsync(t => t.Name == request.Name);
        if (existing != null) return CatalogMutationResult.BadRequest("Ya existe una configuración fiscal con ese nombre");

        var tax = new TaxConfiguration
        {
            Name = request.Name,
            Rate = request.Rate,
            IsActive = request.IsActive,
            ApplicableTo = request.ApplicableTo
        };
        await _context.TaxConfigurations.AddAsync(tax);
        await _mutations.PersistAsync(actor, "CreateTaxConfiguration", nameof(TaxConfiguration), () => tax.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(tax.Id, _mapper.Map<TaxConfigurationDto>(tax));
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, CreateTaxConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("TaxConfigurations", cancellationToken);

        var tax = await _context.TaxConfigurations.FindAsync(id);
        if (tax == null) return CatalogMutationResult.NotFound();

        tax.Name = request.Name;
        tax.Rate = request.Rate;
        tax.IsActive = request.IsActive;
        tax.ApplicableTo = request.ApplicableTo;
        await _mutations.PersistAsync(actor, "UpdateTaxConfiguration", nameof(TaxConfiguration), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("TaxConfigurations", cancellationToken);

        var tax = await _context.TaxConfigurations.FindAsync(id);
        if (tax == null) return CatalogMutationResult.NotFound();

        tax.IsDeleted = true;
        await _mutations.PersistAsync(actor, "DeleteTaxConfiguration", nameof(TaxConfiguration), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
