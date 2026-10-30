using hotel_erp.Api.Dtos.Customer;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class CustomerApplicationService(
    ICustomerRepository _repo, IInvoiceRepository _invoiceRepo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Customers", cancellationToken);

        if (!string.IsNullOrEmpty(request.RTN))
        {
            var existing = await _repo.GetByRTNAsync(request.RTN);
            if (existing != null) return CatalogMutationResult.BadRequest("El RTN ya existe");
        }

        var taxpayerType = Enum.TryParse<TaxpayerType>(request.TaxpayerType, out var parsedType) ? parsedType : TaxpayerType.Gravado;
        if (taxpayerType == TaxpayerType.Exonerado && (string.IsNullOrWhiteSpace(request.ExonerationOrderNumber) || string.IsNullOrWhiteSpace(request.SefinExonerationCertificateNumber)))
            return CatalogMutationResult.BadRequest("Cliente exonerado requiere O.C. Exenta y Constancia SEFIN");

        var entity = new hotel_erp.Api.Database.Entities.Customer
        {
            RTN = request.RTN,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
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
        await _mutations.PersistAsync(actor, "CreateCustomer", nameof(Customer), () => entity.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(entity.Id, _mapper.Map<CustomerDto>(entity));
    }

    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Customers", cancellationToken);

        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return CatalogMutationResult.NotFound();

        if (request.RTN != null) entity.RTN = request.RTN;
        if (request.Name != null) entity.Name = request.Name;
        if (request.Address != null) entity.Address = request.Address;
        if (request.Phone != null) entity.Phone = request.Phone;
        if (request.Email != null) entity.Email = request.Email;
        if (request.TaxpayerType != null && Enum.TryParse<TaxpayerType>(request.TaxpayerType, out var taxpayerType)) entity.TaxpayerType = taxpayerType;
        if (request.ExonerationOrderNumber != null) entity.ExonerationOrderNumber = request.ExonerationOrderNumber;
        if (request.SefinExonerationCertificateNumber != null) entity.SefinExonerationCertificateNumber = request.SefinExonerationCertificateNumber;
        if (request.SagRegistryNumber != null) entity.SagRegistryNumber = request.SagRegistryNumber;
        if (request.IsIsvExempt.HasValue) entity.IsIsvExempt = request.IsIsvExempt.Value;
        if (request.IsTouristTaxExempt.HasValue) entity.IsTouristTaxExempt = request.IsTouristTaxExempt.Value;
        if (request.ExonerationValidFrom.HasValue) entity.ExonerationValidFrom = request.ExonerationValidFrom;
        if (request.ExonerationValidTo.HasValue) entity.ExonerationValidTo = request.ExonerationValidTo;
        if (entity.TaxpayerType == TaxpayerType.Exonerado && (string.IsNullOrWhiteSpace(entity.ExonerationOrderNumber) || string.IsNullOrWhiteSpace(entity.SefinExonerationCertificateNumber)))
            return CatalogMutationResult.BadRequest("Cliente exonerado requiere O.C. Exenta y Constancia SEFIN");
        await _repo.UpdateAsync(entity);
        await _mutations.PersistAsync(actor, "UpdateCustomer", nameof(Customer), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageReservations, cancellationToken);
        await _mutations.LockAsync("Customers", cancellationToken);

        var invoices = await _invoiceRepo.GetByCustomerAsync(id);
        if (invoices.Any())
            return CatalogMutationResult.BadRequest("No se puede eliminar el cliente porque tiene facturas emitidas (trazabilidad fiscal).");

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteCustomer", nameof(Customer), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
