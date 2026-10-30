using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Services;

public sealed class DocumentAuthorizationApplicationService(
    IDocumentAuthorizationRepository _repo, IMapper _mapper, AuthorizationAttachmentStore _attachmentStore, IApplicationFailureInjector _failures,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> CreateAsync(SecurityActor actor, CreateDocumentAuthorizationRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("DocumentAuthorizations", cancellationToken);

        if (!Enum.TryParse<InvoiceDocumentType>(request.DocumentType, out var type)
            || type is not (InvoiceDocumentType.Factura or InvoiceDocumentType.NotaCredito or InvoiceDocumentType.NotaDebito))
            return CatalogMutationResult.BadRequest("Tipo de documento fiscal inválido");

        var existing = await _repo.GetByCAIAsync(type, request.CAINumber);
        if (existing != null) return CatalogMutationResult.BadRequest("Ya existe esa autorización para el tipo de documento indicado");

        var authorization = new DocumentAuthorization
        {
            DocumentType = type,
            CAINumber = request.CAINumber,
            IssueDate = request.IssueDate,
            DueDate = request.DueDate,
            InitialRange = request.InitialRange,
            FinalRange = request.FinalRange,
            CurrentCorrelative = request.InitialRange,
            Status = CAIStatus.Activo
        };

        await _repo.AddAsync(authorization);
        await _mutations.PersistAsync(actor, "CreateDocumentAuthorization", nameof(DocumentAuthorization), () => authorization.Id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Created(authorization.Id, _mapper.Map<DocumentAuthorizationDto>(authorization));
    }

    public async Task<CatalogMutationResult> DeleteAsync(SecurityActor actor, Guid id, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("DocumentAuthorizations", cancellationToken);

        await _repo.DeleteAsync(id);
        await _mutations.PersistAsync(actor, "DeleteDocumentAuthorization", nameof(DocumentAuthorization), () => id, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.NoContent();
    }

    public async Task<CatalogMutationResult> CreateWithFileAsync(SecurityActor actor, CreateDocumentAuthorizationRequest request, IFormFile? file, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("DocumentAuthorizations", cancellationToken);

        if (!Enum.TryParse<InvoiceDocumentType>(request.DocumentType, out var type)
            || type is not (InvoiceDocumentType.Factura or InvoiceDocumentType.NotaCredito or InvoiceDocumentType.NotaDebito))
            return CatalogMutationResult.BadRequest("Tipo de documento fiscal inválido");

        var existing = await _repo.GetByCAIAsync(type, request.CAINumber);
        if (existing != null) return CatalogMutationResult.BadRequest("Ya existe esa autorización para el tipo de documento indicado");

        var authorization = new DocumentAuthorization
        {
            Id = Guid.NewGuid(),
            DocumentType = type,
            CAINumber = request.CAINumber,
            IssueDate = request.IssueDate,
            DueDate = request.DueDate,
            InitialRange = request.InitialRange,
            FinalRange = request.FinalRange,
            CurrentCorrelative = request.InitialRange,
            Status = CAIStatus.Activo
        };

        if (file is not null)
        {
            try
            {
                authorization.AttachmentPath = await _attachmentStore.SavePdfAsync(
                    authorization.Id,
                    file,
                    cancellationToken);
            }
            catch (AttachmentValidationException exception)
            {
                return CatalogMutationResult.BadRequest(new CatalogProblem
                {
                    Type = "https://httpstatuses.com/400",
                    Title = "Adjunto rechazado",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = exception.Message
                });
            }
        }

        var commitAttempted = false;
        try
        {
            await _failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterAttachment, cancellationToken);
            await _repo.AddAsync(authorization);
            await _mutations.PersistAsync(actor, "CreateWithFileDocumentAuthorization", nameof(DocumentAuthorization), () => authorization.Id, cancellationToken);
            var dto = _mapper.Map<DocumentAuthorizationDto>(authorization);
            cancellationToken.ThrowIfCancellationRequested();
            // Once COMMIT starts its outcome may be uncertain; retain the PDF rather than delete a possibly referenced file.
            commitAttempted = true;
            await transaction.CommitAsync(cancellationToken);
            return CatalogMutationResult.Created(authorization.Id, dto);
        }
        catch
        {
            if (!commitAttempted) _attachmentStore.DeleteIfExists(authorization.AttachmentPath);
            throw;
        }

    }
}
