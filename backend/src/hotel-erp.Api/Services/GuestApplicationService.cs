using hotel_erp.Api.Dtos.Customer;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class GuestApplicationService(
    IGuestRepository _repo, IReservationRepository _reservationRepo, IInvoiceRepository _invoiceRepo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateGuestRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Guests", cancellationToken);

        if (!string.IsNullOrEmpty(request.DocumentNumber))
        {
            var existing = await _repo.GetByDocumentNumberAsync(request.DocumentNumber);
            if (existing != null) return CatalogMutationResult.BadRequest("El número de documento ya existe");
        }

        var taxpayerType = Enum.TryParse<TaxpayerType>(request.TaxpayerType, out var parsedType) ? parsedType : TaxpayerType.ConsumidorFinal;
        if (taxpayerType == TaxpayerType.Exonerado && (string.IsNullOrWhiteSpace(request.ExonerationOrderNumber) || string.IsNullOrWhiteSpace(request.SefinExonerationCertificateNumber)))
            return CatalogMutationResult.BadRequest("Huésped exonerado requiere O.C. Exenta y Constancia SEFIN");

        var entity = new hotel_erp.Api.Database.Entities.Guest
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            Nationality = request.Nationality,
            DocumentType = request.DocumentType,
            DocumentNumber = request.DocumentNumber,
            Origin = request.Origin,
            HasVehicle = request.HasVehicle,
            VehiclePlate = request.VehiclePlate,
            Company = request.Company,
            RTN = request.GuestRTN,
            Preferences = request.Preferences,
            Classification = request.Classification,
            TaxpayerType = taxpayerType,
            ExonerationOrderNumber = request.ExonerationOrderNumber,
            SefinExonerationCertificateNumber = request.SefinExonerationCertificateNumber,
            SagRegistryNumber = request.SagRegistryNumber,
            IsIsvExempt = request.IsIsvExempt,
            IsTouristTaxExempt = request.IsTouristTaxExempt,
            ExonerationValidFrom = request.ExonerationValidFrom,
            ExonerationValidTo = request.ExonerationValidTo
        };
        await _repo.AddAsync(entity);
        await _mutations.PersistAsync(actor, "CreateGuest", nameof(Guest), () => entity.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(entity.Id, _mapper.Map<GuestDto>(entity));
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, UpdateGuestRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Guests", cancellationToken);

        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return CatalogMutationResult.NotFound();

        if (request.FirstName != null) entity.FirstName = request.FirstName;
        if (request.LastName != null) entity.LastName = request.LastName;
        if (request.Email != null) entity.Email = request.Email;
        if (request.Phone != null) entity.Phone = request.Phone;
        if (request.DateOfBirth.HasValue) entity.DateOfBirth = request.DateOfBirth;
        if (request.Nationality != null) entity.Nationality = request.Nationality;
        if (request.DocumentType != null) entity.DocumentType = request.DocumentType;
        if (request.DocumentNumber != null) entity.DocumentNumber = request.DocumentNumber;
        if (request.Origin != null) entity.Origin = request.Origin;
        if (request.HasVehicle.HasValue) entity.HasVehicle = request.HasVehicle.Value;
        if (request.VehiclePlate != null) entity.VehiclePlate = request.VehiclePlate;
        if (request.Company != null) entity.Company = request.Company;
        if (request.GuestRTN != null) entity.RTN = request.GuestRTN;
        if (request.Preferences != null) entity.Preferences = request.Preferences;
        if (request.Classification != null) entity.Classification = request.Classification;
        if (request.TaxpayerType != null && Enum.TryParse<TaxpayerType>(request.TaxpayerType, out var taxpayerType)) entity.TaxpayerType = taxpayerType;
        if (request.ExonerationOrderNumber != null) entity.ExonerationOrderNumber = request.ExonerationOrderNumber;
        if (request.SefinExonerationCertificateNumber != null) entity.SefinExonerationCertificateNumber = request.SefinExonerationCertificateNumber;
        if (request.SagRegistryNumber != null) entity.SagRegistryNumber = request.SagRegistryNumber;
        if (request.IsIsvExempt.HasValue) entity.IsIsvExempt = request.IsIsvExempt.Value;
        if (request.IsTouristTaxExempt.HasValue) entity.IsTouristTaxExempt = request.IsTouristTaxExempt.Value;
        if (request.ExonerationValidFrom.HasValue) entity.ExonerationValidFrom = request.ExonerationValidFrom;
        if (request.ExonerationValidTo.HasValue) entity.ExonerationValidTo = request.ExonerationValidTo;
        if (entity.TaxpayerType == TaxpayerType.Exonerado && (string.IsNullOrWhiteSpace(entity.ExonerationOrderNumber) || string.IsNullOrWhiteSpace(entity.SefinExonerationCertificateNumber)))
            return CatalogMutationResult.BadRequest("Huésped exonerado requiere O.C. Exenta y Constancia SEFIN");
        await _repo.UpdateAsync(entity);
        await _mutations.PersistAsync(actor, "UpdateGuest", nameof(Guest), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> UpdateClassificationAsync(SecurityActor actor, Guid id, UpdateClassificationRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Guests", cancellationToken);

        var guest = await _repo.GetByIdAsync(id);
        if (guest == null) return CatalogMutationResult.NotFound();
        guest.Classification = request.Classification;
        await _repo.UpdateAsync(guest);
        await _mutations.PersistAsync(actor, "UpdateClassificationGuest", nameof(Guest), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Ok(new { classification = guest.Classification });
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Guests", cancellationToken);

        var reservations = await _reservationRepo.GetByGuestAsync(id);
        if (reservations.Any())
            return CatalogMutationResult.BadRequest("No se puede eliminar el huésped porque tiene reservaciones asociadas.");

        var invoices = await _invoiceRepo.GetByGuestAsync(id);
        if (invoices.Any())
            return CatalogMutationResult.BadRequest("No se puede eliminar el huésped porque tiene facturas emitidas (trazabilidad fiscal).");

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteGuest", nameof(Guest), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
