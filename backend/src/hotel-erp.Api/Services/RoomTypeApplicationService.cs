using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class RoomTypeApplicationService(
    IRoomTypeRepository _repo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateRoomTypeRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("RoomTypes", cancellationToken);

        var existing = await _repo.GetByNameAsync(request.Name);
        if (existing != null) return CatalogMutationResult.BadRequest("El tipo de habitación ya existe");

        var entity = new hotel_erp.Api.Database.Entities.RoomType
        {
            Name = request.Name,
            Description = request.Description,
            PricePerNight = request.PricePerNight,
            Capacity = request.Capacity
        };
        await _repo.AddAsync(entity);
        await _mutations.PersistAsync(actor, "CreateRoomType", nameof(RoomType), () => entity.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(entity.Id, _mapper.Map<RoomTypeDto>(entity));
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, UpdateRoomTypeRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("RoomTypes", cancellationToken);

        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return CatalogMutationResult.NotFound();

        if (request.Name != null) entity.Name = request.Name;
        if (request.Description != null) entity.Description = request.Description;
        if (request.PricePerNight.HasValue) entity.PricePerNight = request.PricePerNight.Value;
        if (request.Capacity.HasValue) entity.Capacity = request.Capacity.Value;
        await _repo.UpdateAsync(entity);
        await _mutations.PersistAsync(actor, "UpdateRoomType", nameof(RoomType), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("RoomTypes", cancellationToken);

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteRoomType", nameof(RoomType), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
