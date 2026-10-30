using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed class BusinessSettingsApplicationService(
    IBusinessSettingsRepository _repo, IMapper _mapper, IAuditService _auditService, ApplicationDbContext _context, IHotelClock _clock, IUnitOfWork _unitOfWork, IApplicationFailureInjector _failures,
    OperationalMutationGuard access, CatalogMutationCommitter _mutations)
{
    public async Task<CatalogMutationResult> UpdateAsync(SecurityActor actor, UpdateBusinessSettingsRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageSettings, cancellationToken);
        await _mutations.LockAsync("Settings", cancellationToken);

        var existing = await _repo.GetAsync() ?? new BusinessSettings();

        var fiscalChanged =
            (request.BusinessName != null && request.BusinessName.Trim() != existing.BusinessName)
            || (request.RTN != null && request.RTN.Trim() != existing.RTN)
            || (request.Address != null && request.Address.Trim() != existing.Address)
            || (request.Phone != null && request.Phone.Trim() != existing.Phone)
            || (request.Email != null && request.Email.Trim().ToLowerInvariant() != existing.Email)
            || (request.IsvRate.HasValue && request.IsvRate.Value != existing.IsvRate)
            || (request.TouristTaxRate.HasValue && request.TouristTaxRate.Value != existing.TouristTaxRate)
            || (request.FiscalValidFrom.HasValue && request.FiscalValidFrom != existing.FiscalValidFrom)
            || (request.FiscalValidUntil.HasValue && request.FiscalValidUntil != existing.FiscalValidUntil);

        if (request.BusinessName != null) existing.BusinessName = request.BusinessName.Trim();
        if (request.RTN != null) existing.RTN = request.RTN.Trim();
        if (request.Address != null) existing.Address = request.Address.Trim();
        if (request.Phone != null) existing.Phone = request.Phone.Trim();
        if (request.Email != null) existing.Email = request.Email.Trim().ToLowerInvariant();
        if (request.LogoBase64 != null) existing.LogoBase64 = request.LogoBase64;
        if (request.Footer != null) existing.Footer = request.Footer;
        if (request.IsvRate.HasValue) existing.IsvRate = request.IsvRate.Value;
        if (request.TouristTaxRate.HasValue) existing.TouristTaxRate = request.TouristTaxRate.Value;
        if (request.FiscalValidFrom.HasValue) existing.FiscalValidFrom = request.FiscalValidFrom;
        if (request.FiscalValidUntil.HasValue) existing.FiscalValidUntil = request.FiscalValidUntil;
        if (request.PrintPrinterName != null) existing.PrintPrinterName = request.PrintPrinterName;
        if (request.PrintWidth.HasValue) existing.PrintWidth = request.PrintWidth.Value;
        if (request.PrintLogoHeight.HasValue) existing.PrintLogoHeight = request.PrintLogoHeight.Value;
        if (request.PrintFontSize != null) existing.PrintFontSize = request.PrintFontSize;
        if (request.PrintLineSpacing.HasValue) existing.PrintLineSpacing = request.PrintLineSpacing.Value;
        if (request.ShowLogo.HasValue) existing.ShowLogo = request.ShowLogo.Value;
        if (request.ShowHeader.HasValue) existing.ShowHeader = request.ShowHeader.Value;
        if (request.ShowFiscal.HasValue) existing.ShowFiscal = request.ShowFiscal.Value;
        if (request.ShowGuest.HasValue) existing.ShowGuest = request.ShowGuest.Value;
        if (request.ShowItems.HasValue) existing.ShowItems = request.ShowItems.Value;
        if (request.ShowTotals.HasValue) existing.ShowTotals = request.ShowTotals.Value;
        if (request.ShowPayment.HasValue) existing.ShowPayment = request.ShowPayment.Value;
        if (request.ShowFooter.HasValue) existing.ShowFooter = request.ShowFooter.Value;
        if (request.HeaderAlign != null) existing.HeaderAlign = request.HeaderAlign;
        if (request.SeparatorChar != null) existing.SeparatorChar = request.SeparatorChar;
        if (request.MarginLeft.HasValue) existing.MarginLeft = request.MarginLeft.Value;

        if (fiscalChanged)
        {
            if (existing.FiscalProfileStatus != FiscalProfileStatus.Borrador)
                existing.FiscalProfileVersion++;
            existing.FiscalProfileStatus = FiscalProfileStatus.Borrador;
            existing.FiscalApprovedAt = null;
            existing.FiscalApprovedByUserId = null;
            existing.FiscalApprovalNote = null;
            existing.FiscalRetiredAt = null;
            existing.FiscalRetiredByUserId = null;
            existing.FiscalRetirementReason = null;
            existing.FiscalDecisionManifestSha256 = null;
            existing.FiscalEvidenceReference = null;
        }

        await _repo.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterPersistence, cancellationToken);
        var actorId = actor.Id;
        await _auditService.LogAsync(actorId, "UpdateBusinessSettings", nameof(BusinessSettings), existing.Id, new
        {
            existing.FiscalProfileStatus,
            existing.FiscalProfileVersion,
            FiscalDataChanged = fiscalChanged
        });
        await _failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterAudit, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Ok(_mapper.Map<BusinessSettingsDto>(existing));
    }

    public async Task<CatalogMutationResult> ApproveFiscalProfileAsync(SecurityActor actor, ApproveFiscalProfileRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("Settings", cancellationToken);

        var settings = await _repo.GetAsync();
        if (settings is null)
            return CatalogMutationResult.BadRequest(new { message = "Primero debe guardar la configuración del negocio" });

        if (settings.FiscalProfileStatus == FiscalProfileStatus.Retirado)
            settings.FiscalProfileVersion++;

        var revisionExists = await _context.FiscalProfileRevisions
            .AnyAsync(revision => revision.BusinessSettingsId == settings.Id
                && revision.Version == settings.FiscalProfileVersion);
        if (revisionExists)
            return CatalogMutationResult.Conflict(new CatalogProblem
            {
                Type = "https://httpstatuses.com/409",
                Title = "Versión fiscal inmutable",
                Status = StatusCodes.Status409Conflict,
                Detail = "Esta versión ya fue aprobada. Modifique los datos fiscales para crear un nuevo borrador."
            });

        settings.FiscalValidFrom = request.ValidFrom;
        settings.FiscalValidUntil = request.ValidUntil;
        settings.FiscalDecisionManifestSha256 = request.DecisionManifestSha256.ToLowerInvariant();
        settings.FiscalEvidenceReference = request.EvidenceReference.Trim();
        var validationErrors = FiscalProfileService.ValidateForApproval(settings);
        if (validationErrors.Count > 0)
            return CatalogMutationResult.BadRequest(new { message = "El perfil fiscal está incompleto", errors = validationErrors });

        var actorId = actor.Id;
        settings.FiscalProfileStatus = FiscalProfileStatus.Aprobado;
        settings.FiscalApprovedAt = _clock.UtcNow;
        settings.FiscalApprovedByUserId = actorId;
        settings.FiscalApprovalNote = request.ApprovalNote.Trim();
        settings.FiscalRetiredAt = null;
        settings.FiscalRetiredByUserId = null;
        settings.FiscalRetirementReason = null;
        _context.FiscalProfileRevisions.Add(FiscalProfileService.CreateRevision(settings, actorId));
        try
        {
            await _repo.UpdateAsync(settings);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterPersistence, cancellationToken);
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync(cancellationToken);
            return CatalogMutationResult.Conflict(new CatalogProblem
            {
                Type = "https://httpstatuses.com/409",
                Title = "Aprobación fiscal concurrente",
                Status = StatusCodes.Status409Conflict,
                Detail = "Otro proceso aprobó esta versión. Recargue el perfil antes de continuar."
            });
        }
        await _auditService.LogAsync(actorId, "ApproveFiscalProfile", nameof(BusinessSettings), settings.Id, new
        {
            settings.FiscalProfileVersion,
            settings.FiscalValidFrom,
            settings.FiscalValidUntil,
            settings.FiscalApprovalNote,
            settings.FiscalDecisionManifestSha256,
            settings.FiscalEvidenceReference
        });
        await _failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterAudit, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Ok(_mapper.Map<BusinessSettingsDto>(settings));
    }

    public async Task<CatalogMutationResult> RetireFiscalProfileAsync(SecurityActor actor, RetireFiscalProfileRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await access.BeginAsync(actor, PermissionNames.ManageTaxes, cancellationToken);
        await _mutations.LockAsync("Settings", cancellationToken);

        var settings = await _repo.GetAsync();
        if (settings is null)
            return CatalogMutationResult.NotFound();

        if (settings.FiscalProfileStatus != FiscalProfileStatus.Aprobado)
            return CatalogMutationResult.Conflict(new CatalogProblem
            {
                Type = "https://httpstatuses.com/409",
                Title = "Perfil fiscal no aprobado",
                Status = StatusCodes.Status409Conflict,
                Detail = "Solo una versión aprobada puede retirarse."
            });

        var revision = await _context.FiscalProfileRevisions
            .SingleOrDefaultAsync(candidate => candidate.BusinessSettingsId == settings.Id
                && candidate.Version == settings.FiscalProfileVersion);
        if (revision is null)
            return CatalogMutationResult.Conflict(new CatalogProblem
            {
                Type = "https://httpstatuses.com/409",
                Title = "Historial fiscal incompleto",
                Status = StatusCodes.Status409Conflict,
                Detail = "La versión aprobada no tiene snapshot histórico; registre nuevamente su aprobación antes de retirarla."
            });

        var actorId = actor.Id;
        settings.FiscalProfileStatus = FiscalProfileStatus.Retirado;
        settings.FiscalRetiredAt = _clock.UtcNow;
        settings.FiscalRetiredByUserId = actorId;
        settings.FiscalRetirementReason = request.Reason.Trim();
        revision.Status = FiscalProfileStatus.Retirado;
        revision.RetiredAt = settings.FiscalRetiredAt;
        revision.RetiredByUserId = actorId;
        revision.RetirementReason = settings.FiscalRetirementReason;
        await _repo.UpdateAsync(settings);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterPersistence, cancellationToken);
        await _auditService.LogAsync(actorId, "RetireFiscalProfile", nameof(BusinessSettings), settings.Id, new
        {
            settings.FiscalProfileVersion,
            settings.FiscalRetirementReason
        });
        await _failures.CheckpointAsync(ApplicationFailureCheckpoints.CatalogAfterAudit, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return CatalogMutationResult.Ok(_mapper.Map<BusinessSettingsDto>(settings));
    }
}
