using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class CaiApplicationService(
    ICAIRepository _repo, IMapper _mapper,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateCAIRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("CAI", cancellationToken);

        var existing = await _repo.GetByCAINumberAsync(request.CAINumber);
        if (existing != null) return CatalogMutationResult.BadRequest("El número de CAI ya existe");

        var activeCai = await _repo.GetActiveCAIAsync();
        if (activeCai != null) return CatalogMutationResult.BadRequest("Ya existe un CAI activo. Desactívelo antes de crear uno nuevo.");

        var cai = new CAI
        {
            CAINumber = request.CAINumber,
            IssueDate = request.IssueDate,
            DueDate = request.DueDate,
            InitialRange = request.InitialRange,
            FinalRange = request.FinalRange,
            CurrentCorrelative = request.InitialRange,
            Status = CAIStatus.Activo
        };

        await _repo.AddAsync(cai);
        await _mutations.PersistAsync(actor, "CreateCAI", nameof(CAI), () => cai.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(cai.Id, _mapper.Map<CAIDto>(cai));
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("CAI", cancellationToken);

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteCAI", nameof(CAI), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }
}
