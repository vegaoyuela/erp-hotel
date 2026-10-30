using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class RoomApplicationService(
    IRoomRepository _repo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateRoomRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Rooms", cancellationToken);

        var existing = await _repo.GetByRoomNumberAsync(request.RoomNumber);
        if (existing != null) return CatalogMutationResult.BadRequest("El número de habitación ya existe");

        var room = new hotel_erp.Api.Database.Entities.Room
        {
            RoomNumber = request.RoomNumber,
            Floor = request.Floor,
            RoomTypeId = request.RoomTypeId,
            Observations = request.Observations
        };
        await _repo.AddAsync(room);
        await _mutations.PersistAsync(actor, "CreateRoom", nameof(Room), () => room.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(room.Id, _mapper.Map<RoomDto>(room));
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, UpdateRoomRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Rooms", cancellationToken);

        var room = await _repo.GetByIdAsync(id);
        if (room == null) return CatalogMutationResult.NotFound();

        if (request.RoomNumber != null) room.RoomNumber = request.RoomNumber;
        if (request.Floor.HasValue) room.Floor = request.Floor.Value;
        if (request.RoomTypeId.HasValue) room.RoomTypeId = request.RoomTypeId.Value;
        if (request.Status != null && Enum.TryParse<hotel_erp.Api.Database.Entities.RoomStatus>(request.Status, out var parsedStatus))
            room.Status = parsedStatus;
        if (request.Observations != null) room.Observations = request.Observations;
        await _repo.UpdateAsync(room);
        await _mutations.PersistAsync(actor, "UpdateRoom", nameof(Room), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Rooms", cancellationToken);

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteRoom", nameof(Room), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
