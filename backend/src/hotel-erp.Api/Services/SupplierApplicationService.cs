using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class SupplierApplicationService(
    ISupplierRepository _repo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);
        await _mutations.LockAsync("Suppliers", cancellationToken);

        var supplier = new hotel_erp.Api.Database.Entities.Supplier
        {
            Name = request.Name,
            RTN = request.RTN,
            ContactPerson = request.ContactPerson,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address
        };
        await _repo.AddAsync(supplier);
        await _mutations.PersistAsync(actor, "CreateSupplier", nameof(Supplier), () => supplier.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(supplier.Id, _mapper.Map<SupplierDto>(supplier));
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, UpdateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);
        await _mutations.LockAsync("Suppliers", cancellationToken);

        var supplier = await _repo.GetByIdAsync(id);
        if (supplier == null) return CatalogMutationResult.NotFound();

        if (request.Name != null) supplier.Name = request.Name;
        if (request.RTN != null) supplier.RTN = request.RTN;
        if (request.ContactPerson != null) supplier.ContactPerson = request.ContactPerson;
        if (request.Phone != null) supplier.Phone = request.Phone;
        if (request.Email != null) supplier.Email = request.Email;
        if (request.Address != null) supplier.Address = request.Address;
        if (request.IsActive.HasValue) supplier.IsActive = request.IsActive.Value;

        await _repo.UpdateAsync(supplier);
        await _mutations.PersistAsync(actor, "UpdateSupplier", nameof(Supplier), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageAccounting, cancellationToken);
        await _mutations.LockAsync("Suppliers", cancellationToken);

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteSupplier", nameof(Supplier), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
