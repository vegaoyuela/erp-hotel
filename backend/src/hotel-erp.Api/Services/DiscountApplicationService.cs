using hotel_erp.Api.Dtos.Discount;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class DiscountApplicationService(
    IDiscountRepository _repo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateDiscountRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageSettings, cancellationToken);
        await _mutations.LockAsync("Discounts", cancellationToken);

        var entity = new Discount
        {
            Name = request.Name,
            Description = request.Description,
            DiscountType = Enum.Parse<DiscountType>(request.DiscountType),
            Value = request.Value,
            IsActive = request.IsActive,
            ApplicableTo = request.ApplicableTo,
            RequiresDocument = request.RequiresDocument,
            MinAge = request.MinAge,
            Priority = request.Priority
        };
        await _repo.AddAsync(entity);
        await _mutations.PersistAsync(actor, "CreateDiscount", nameof(Discount), () => entity.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(entity.Id, _mapper.Map<DiscountDto>(entity));
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, UpdateDiscountRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageSettings, cancellationToken);
        await _mutations.LockAsync("Discounts", cancellationToken);

        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return CatalogMutationResult.NotFound();

        if (request.Name != null) entity.Name = request.Name;
        if (request.Description != null) entity.Description = request.Description;
        if (request.DiscountType != null) entity.DiscountType = Enum.Parse<DiscountType>(request.DiscountType);
        if (request.Value.HasValue) entity.Value = request.Value.Value;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;
        if (request.ApplicableTo != null) entity.ApplicableTo = request.ApplicableTo;
        if (request.RequiresDocument.HasValue) entity.RequiresDocument = request.RequiresDocument.Value;
        if (request.MinAge.HasValue) entity.MinAge = request.MinAge.Value;
        if (request.Priority.HasValue) entity.Priority = request.Priority.Value;

        await _repo.UpdateAsync(entity);
        await _mutations.PersistAsync(actor, "UpdateDiscount", nameof(Discount), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageSettings, cancellationToken);
        await _mutations.LockAsync("Discounts", cancellationToken);

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteDiscount", nameof(Discount), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
