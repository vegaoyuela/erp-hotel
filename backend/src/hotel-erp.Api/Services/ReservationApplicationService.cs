using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record ReservationVersionResult(int Version);
public sealed record ReservationStateResult(string Message, int Version);
public sealed record CheckInResult(
    string Message,
    Guid FolioId,
    int Version,
    decimal SellingPricePerNight,
    int Nights,
    decimal Subtotal,
    decimal Isv,
    decimal IsvRate,
    decimal TouristTax,
    decimal TouristTaxRate,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal Total);
public sealed record CheckOutResult(string Message, Guid InvoiceId);

public sealed class ReservationApplicationService(
    ApplicationDbContext context,
    IReservationRepository repository,
    IBusinessSettingsRepository settingsRepository,
    IDiscountRepository discountRepository,
    TaxService taxService,
    AuditService auditService,
    IHotelClock clock,
    IApplicationFailureInjector failureInjector)
{
    public async Task<ApplicationMutationResult<Reservation>> CreateAsync(
        Guid userId,
        CreateReservationRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.AdvancePayment > 0m)
        {
            return ApplicationMutationResult<Reservation>.BadRequest(
                "Los anticipos estarán disponibles cuando puedan registrarse y aplicarse contablemente");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockRoomScheduleAsync(request.RoomId, cancellationToken);

        var room = await context.Rooms
            .Include(candidate => candidate.RoomType)
            .SingleOrDefaultAsync(candidate => candidate.Id == request.RoomId, cancellationToken);
        if (room is null)
            return ApplicationMutationResult<Reservation>.NotFound("Habitación no encontrada");
        if (room.Status is RoomStatus.Mantenimiento or RoomStatus.Bloqueada)
            return ApplicationMutationResult<Reservation>.Conflict($"La habitación no admite reservas: {room.Status}");
        if (request.Adults + request.Children > room.RoomType.Capacity)
            return ApplicationMutationResult<Reservation>.BadRequest("La ocupación supera la capacidad de la habitación");

        var guest = await context.Guests
            .SingleOrDefaultAsync(candidate => candidate.Id == request.GuestId, cancellationToken);
        if (guest is null)
            return ApplicationMutationResult<Reservation>.NotFound("Huésped no encontrado");
        if (await repository.HasOverlapAsync(
                request.RoomId,
                request.CheckInDate,
                request.CheckOutDate,
                Guid.Empty))
        {
            return ApplicationMutationResult<Reservation>.Conflict(
                "La habitación ya tiene una reserva activa que se superpone con las fechas");
        }

        var reservation = new Reservation
        {
            Guest = guest,
            Room = room,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Adults = request.Adults,
            Children = request.Children,
            PaymentMethod = request.PaymentMethod,
            AdvancePayment = 0m,
            Notes = request.Notes,
            Status = ReservationStatus.Pendiente,
            Version = 1
        };

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.ReservationCreateAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateReservation",
            nameof(Reservation),
            reservation.Id,
            new
            {
                reservation.GuestId,
                reservation.RoomId,
                reservation.CheckInDate,
                reservation.CheckOutDate,
                reservation.Adults,
                reservation.Children,
                reservation.Version
            });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<Reservation>.Success(reservation);
    }

    public async Task<ApplicationMutationResult<ReservationVersionResult>> UpdateAsync(
        Guid id,
        Guid userId,
        UpdateReservationRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockReservationAsync(id, cancellationToken);
        var reservation = await context.Reservations
            .Include(candidate => candidate.Guest)
            .Include(candidate => candidate.Room)
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (reservation is null)
            return ApplicationMutationResult<ReservationVersionResult>.NotFound("Reserva no encontrada");
        if (reservation.Status is not (ReservationStatus.Pendiente or ReservationStatus.Confirmada))
        {
            return ApplicationMutationResult<ReservationVersionResult>.Conflict(
                $"No se puede modificar una reserva en estado {reservation.Status}");
        }
        if (reservation.Version != request.ExpectedVersion)
            return VersionConflict<ReservationVersionResult>(reservation.Version);
        if (request.AdvancePayment is > 0m)
        {
            return ApplicationMutationResult<ReservationVersionResult>.BadRequest(
                "Los anticipos estarán disponibles cuando puedan registrarse y aplicarse contablemente");
        }

        var roomId = request.RoomId ?? reservation.RoomId;
        var checkInDate = request.CheckInDate ?? reservation.CheckInDate;
        var checkOutDate = request.CheckOutDate ?? reservation.CheckOutDate;
        var adults = request.Adults ?? reservation.Adults;
        var children = request.Children ?? reservation.Children;
        if (checkOutDate <= checkInDate)
        {
            return ApplicationMutationResult<ReservationVersionResult>.BadRequest(
                "La fecha de salida debe ser posterior a la fecha de entrada");
        }

        var roomIds = new[] { reservation.RoomId, roomId }.Distinct().Order().ToList();
        foreach (var lockedRoomId in roomIds)
            await LockRoomScheduleAsync(lockedRoomId, cancellationToken);

        var room = await context.Rooms
            .Include(candidate => candidate.RoomType)
            .SingleOrDefaultAsync(candidate => candidate.Id == roomId, cancellationToken);
        if (room is null)
            return ApplicationMutationResult<ReservationVersionResult>.NotFound("Habitación no encontrada");
        if (room.Status is RoomStatus.Mantenimiento or RoomStatus.Bloqueada)
        {
            return ApplicationMutationResult<ReservationVersionResult>.Conflict(
                $"La habitación no admite reservas: {room.Status}");
        }
        if (adults + children > room.RoomType.Capacity)
            return ApplicationMutationResult<ReservationVersionResult>.BadRequest("La ocupación supera la capacidad de la habitación");
        if (await repository.HasOverlapAsync(roomId, checkInDate, checkOutDate, id))
        {
            return ApplicationMutationResult<ReservationVersionResult>.Conflict(
                "La habitación ya tiene una reserva activa que se superpone con las fechas");
        }

        reservation.RoomId = roomId;
        reservation.Room = room;
        reservation.CheckInDate = checkInDate;
        reservation.CheckOutDate = checkOutDate;
        reservation.Adults = adults;
        reservation.Children = children;
        if (request.PaymentMethod is not null)
            reservation.PaymentMethod = request.PaymentMethod;
        if (request.Notes is not null)
            reservation.Notes = request.Notes;
        reservation.Version++;

        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.ReservationUpdateAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "UpdateReservation",
            nameof(Reservation),
            reservation.Id,
            new
            {
                reservation.RoomId,
                reservation.CheckInDate,
                reservation.CheckOutDate,
                reservation.Adults,
                reservation.Children,
                reservation.Version
            });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<ReservationVersionResult>.Success(new(reservation.Version));
    }

    public async Task<ApplicationMutationResult<ReservationStateResult>> ConfirmAsync(
        Guid id,
        Guid userId,
        ConfirmReservationRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockReservationAsync(id, cancellationToken);
        var reservation = await context.Reservations
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (reservation is null)
            return ApplicationMutationResult<ReservationStateResult>.NotFound("Reserva no encontrada");

        if (reservation.Status == ReservationStatus.Confirmada)
        {
            await transaction.CommitAsync(cancellationToken);
            return ApplicationMutationResult<ReservationStateResult>.Success(
                new("La reserva ya estaba confirmada", reservation.Version));
        }
        if (reservation.Status != ReservationStatus.Pendiente)
        {
            return ApplicationMutationResult<ReservationStateResult>.Conflict(
                $"No se puede confirmar una reserva en estado {reservation.Status}");
        }
        if (reservation.Version != request.ExpectedVersion)
            return VersionConflict<ReservationStateResult>(reservation.Version);

        reservation.Status = ReservationStatus.Confirmada;
        reservation.Version++;
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.ReservationConfirmAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "ConfirmReservation",
            nameof(Reservation),
            reservation.Id,
            new { reservation.Status, reservation.Version });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<ReservationStateResult>.Success(
            new("Reserva confirmada", reservation.Version));
    }

    public async Task<ApplicationMutationResult<ReservationStateResult>> CancelAsync(
        Guid id,
        Guid userId,
        CancelReservationRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockReservationAsync(id, cancellationToken);
        var reservation = await context.Reservations
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (reservation is null)
            return ApplicationMutationResult<ReservationStateResult>.NotFound("Reserva no encontrada");

        if (reservation.Status == ReservationStatus.Cancelada)
        {
            await transaction.CommitAsync(cancellationToken);
            return ApplicationMutationResult<ReservationStateResult>.Success(
                new("La reserva ya estaba cancelada", reservation.Version));
        }
        if (reservation.Status is not (ReservationStatus.Pendiente or ReservationStatus.Confirmada))
        {
            return ApplicationMutationResult<ReservationStateResult>.Conflict(
                $"No se puede cancelar una reserva en estado {reservation.Status}");
        }
        if (reservation.Version != request.ExpectedVersion)
            return VersionConflict<ReservationStateResult>(reservation.Version);

        reservation.Status = ReservationStatus.Cancelada;
        reservation.Version++;
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.ReservationCancelAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CancelReservation",
            nameof(Reservation),
            reservation.Id,
            new { Reason = request.Reason.Trim(), reservation.Status, reservation.Version });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<ReservationStateResult>.Success(
            new("Reserva cancelada", reservation.Version));
    }

    public async Task<ApplicationMutationResult<CheckInResult>> CheckInAsync(
        Guid userId,
        CheckInRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockReservationAsync(request.ReservationId, cancellationToken);
        await LockRoomScheduleAsync(request.RoomId, cancellationToken);

        var reservation = await context.Reservations
            .Include(candidate => candidate.Guest)
            .SingleOrDefaultAsync(candidate => candidate.Id == request.ReservationId, cancellationToken);
        if (reservation is null)
            return ApplicationMutationResult<CheckInResult>.NotFound("Reserva no encontrada");
        if (reservation.Status is not (ReservationStatus.Pendiente or ReservationStatus.Confirmada))
        {
            return ApplicationMutationResult<CheckInResult>.Conflict(
                $"No se puede registrar check-in desde el estado {reservation.Status}");
        }
        if (reservation.Version != request.ExpectedVersion)
            return VersionConflict<CheckInResult>(reservation.Version);
        if (reservation.RoomId != request.RoomId)
            return ApplicationMutationResult<CheckInResult>.BadRequest("La habitación solicitada no coincide con la reserva");
        if (reservation.CheckOutDate <= reservation.CheckInDate)
            return ApplicationMutationResult<CheckInResult>.BadRequest("La estadía debe ser al menos 1 noche");
        if (await context.Folios.IgnoreQueryFilters().AnyAsync(
                folio => folio.ReservationId == reservation.Id,
                cancellationToken))
            return ApplicationMutationResult<CheckInResult>.Conflict("La reserva ya tiene un folio de estancia");

        var settings = await settingsRepository.GetAsync();
        if (settings is not null)
        {
            taxService.IsvRate = settings.IsvRate;
            taxService.TouristTaxRate = settings.TouristTaxRate;
        }

        var room = await context.Rooms
            .Include(candidate => candidate.RoomType)
            .SingleOrDefaultAsync(candidate => candidate.Id == request.RoomId, cancellationToken);
        if (room is null)
            return ApplicationMutationResult<CheckInResult>.NotFound("Habitación no encontrada");
        if (room.Status != RoomStatus.Libre)
        {
            return ApplicationMutationResult<CheckInResult>.Conflict(
                $"La habitación no está disponible físicamente: {room.Status}");
        }
        if (reservation.Adults + reservation.Children > room.RoomType.Capacity)
            return ApplicationMutationResult<CheckInResult>.BadRequest("La ocupación supera la capacidad de la habitación");
        if (await repository.HasOverlapAsync(
                request.RoomId,
                reservation.CheckInDate,
                reservation.CheckOutDate,
                reservation.Id))
            return ApplicationMutationResult<CheckInResult>.Conflict("Existe otra reserva activa que se superpone con la estancia");

        var nights = reservation.CheckOutDate.DayNumber - reservation.CheckInDate.DayNumber;
        var sellingPricePerNight = room.RoomType.PricePerNight;
        var taxResult = taxService.CalculateFromSellingPrice(sellingPricePerNight, nights);

        decimal totalDiscountPercent = 0m;
        if (request.DiscountIds?.Any() == true)
        {
            var discounts = await discountRepository.GetActiveAsync();
            var requestedDiscountIds = request.DiscountIds.Distinct().ToList();
            var selectedDiscounts = discounts
                .Where(discount => requestedDiscountIds.Contains(discount.Id))
                .OrderBy(discount => discount.Priority)
                .ToList();
            if (selectedDiscounts.Count != requestedDiscountIds.Count)
                return ApplicationMutationResult<CheckInResult>.BadRequest("Uno o más descuentos no existen o no están activos");
            if (selectedDiscounts.Any(discount => discount.DiscountType != DiscountType.Porcentaje))
                return ApplicationMutationResult<CheckInResult>.BadRequest("El check-in solo admite descuentos porcentuales");
            totalDiscountPercent = selectedDiscounts.Sum(discount => discount.Value);
            if (totalDiscountPercent > 100m)
                return ApplicationMutationResult<CheckInResult>.BadRequest("La suma de descuentos no puede superar 100 %");
            taxResult = taxService.ApplyDiscount(taxResult, totalDiscountPercent);
        }

        var folio = new Folio
        {
            ReservationId = reservation.Id,
            GuestId = reservation.GuestId,
            RoomId = request.RoomId,
            OpeningDate = clock.HondurasNow,
            Status = FolioStatus.Abierto,
            TotalAmount = taxResult.Total
        };
        folio.FolioItems.Add(new FolioItem
        {
            Description = $"Hospedaje - {room.RoomNumber} x {nights} noche{(nights > 1 ? "s" : string.Empty)}",
            Quantity = 1,
            UnitPrice = taxResult.Subtotal,
            LineTotal = taxResult.Subtotal,
            IsExempt = false,
            ISVRate = taxService.IsvRate,
            IsTouristTaxable = true
        });

        reservation.Status = ReservationStatus.CheckIn;
        reservation.Version++;
        room.Status = RoomStatus.Ocupada;
        context.Folios.Add(folio);

        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.ReservationCheckInAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CheckIn",
            nameof(Reservation),
            reservation.Id,
            new
            {
                reservation.RoomId,
                reservation.CheckInDate,
                reservation.CheckOutDate,
                folio.Id,
                EstimatedTotal = folio.TotalAmount,
                reservation.Version
            });
        await transaction.CommitAsync(cancellationToken);

        return ApplicationMutationResult<CheckInResult>.Success(new(
            "Check-in exitoso",
            folio.Id,
            reservation.Version,
            sellingPricePerNight,
            nights,
            taxResult.Subtotal,
            taxResult.ISV,
            taxService.IsvRate,
            taxResult.TouristTax,
            taxService.TouristTaxRate,
            totalDiscountPercent,
            taxResult.DiscountAmount,
            taxResult.Total));
    }

    public async Task<ApplicationMutationResult<CheckOutResult>> CheckOutAsync(
        Guid id,
        Guid userId,
        CheckOutRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockReservationAsync(id, cancellationToken);

        var reservation = await context.Reservations
            .Include(candidate => candidate.Room)
            .Include(candidate => candidate.Folio)
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (reservation is null)
            return ApplicationMutationResult<CheckOutResult>.NotFound("Reserva no encontrada");
        if (reservation.Folio is null)
            return ApplicationMutationResult<CheckOutResult>.Conflict("La reserva no tiene un folio de estancia");

        var settlementInvoice = await context.Invoices
            .SingleOrDefaultAsync(invoice => invoice.Id == request.InvoiceId, cancellationToken);
        if (settlementInvoice is null
            || settlementInvoice.FolioId != reservation.Folio.Id
            || settlementInvoice.DocumentType != InvoiceDocumentType.Factura)
        {
            return ApplicationMutationResult<CheckOutResult>.Conflict(
                "La factura indicada no liquida el folio de esta reserva");
        }

        if (reservation.Status == ReservationStatus.CheckOut
            && reservation.Folio.Status == FolioStatus.Cerrado)
        {
            await transaction.CommitAsync(cancellationToken);
            return ApplicationMutationResult<CheckOutResult>.Success(
                new("El check-out ya estaba completado", settlementInvoice.Id));
        }
        if (reservation.Status != ReservationStatus.CheckIn || reservation.Folio.Status != FolioStatus.Abierto)
        {
            return ApplicationMutationResult<CheckOutResult>.Conflict(
                "La reserva y su folio no están abiertos para check-out");
        }

        reservation.Folio.ClosingDate = clock.HondurasNow;
        reservation.Folio.Status = FolioStatus.Cerrado;
        reservation.Room.Status = RoomStatus.Limpieza;
        reservation.Status = ReservationStatus.CheckOut;
        reservation.Version++;
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.ReservationCheckOutAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CheckOut",
            nameof(Reservation),
            reservation.Id,
            new
            {
                reservation.Folio.Id,
                InvoiceId = settlementInvoice.Id,
                settlementInvoice.CorrelativeNumber,
                reservation.RoomId,
                reservation.Folio.TotalAmount,
                reservation.Version
            },
            settlementInvoice.CorrelativeNumber,
            settlementInvoice.PaymentMethod);
        await transaction.CommitAsync(cancellationToken);

        return ApplicationMutationResult<CheckOutResult>.Success(
            new("Check-out exitoso, habitación liberada", settlementInvoice.Id));
    }

    private async Task LockReservationAsync(Guid id, CancellationToken cancellationToken)
    {
        var lockKey = $"reservation:{id:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({lockKey}));",
            cancellationToken);
    }

    private async Task LockRoomScheduleAsync(Guid id, CancellationToken cancellationToken)
    {
        var lockKey = $"room-schedule:{id:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({lockKey}));",
            cancellationToken);
    }

    private static ApplicationMutationResult<T> VersionConflict<T>(int currentVersion)
        where T : class
        => ApplicationMutationResult<T>.Conflict(
            $"La reserva cambió desde que fue consultada; versión actual: {currentVersion}");
}
