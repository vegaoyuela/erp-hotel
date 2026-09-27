using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace hotel_erp.Api.Services;

public sealed class FiscalProfileService(IBusinessSettingsRepository settingsRepository, IHotelClock clock)
{
    public async Task<string?> GetOperationBlockReasonAsync()
    {
        var settings = await settingsRepository.GetAsync();
        if (settings is null || settings.FiscalProfileStatus != FiscalProfileStatus.Aprobado)
            return "El perfil fiscal está en borrador o retirado y todavía no permite emitir documentos fiscales.";

        var today = clock.Today;
        if (!settings.FiscalValidFrom.HasValue || settings.FiscalValidFrom.Value > today)
            return "El perfil fiscal aprobado aún no está vigente.";

        if (settings.FiscalValidUntil.HasValue && settings.FiscalValidUntil.Value < today)
            return "La vigencia del perfil fiscal aprobado terminó.";

        return null;
    }

    public static IReadOnlyList<string> ValidateForApproval(BusinessSettings settings)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(settings.BusinessName))
            errors.Add("Debe registrar la razón social o nombre del negocio.");
        if (settings.RTN.Length != 14 || !settings.RTN.All(char.IsDigit))
            errors.Add("Debe registrar un RTN de 14 dígitos.");
        if (string.IsNullOrWhiteSpace(settings.Address))
            errors.Add("Debe registrar la dirección del establecimiento.");
        if (settings.IsvRate < 0 || settings.IsvRate > 1)
            errors.Add("La tasa ISV debe estar entre 0 y 1.");
        if (settings.TouristTaxRate < 0 || settings.TouristTaxRate > 1)
            errors.Add("La tasa turística debe estar entre 0 y 1.");
        if (!settings.FiscalValidFrom.HasValue)
            errors.Add("Debe indicar el inicio de vigencia del perfil fiscal.");
        if (settings.FiscalValidUntil.HasValue
            && settings.FiscalValidFrom.HasValue
            && settings.FiscalValidUntil.Value < settings.FiscalValidFrom.Value)
            errors.Add("La vigencia final no puede ser anterior a la inicial.");
        if (string.IsNullOrWhiteSpace(settings.FiscalDecisionManifestSha256)
            || settings.FiscalDecisionManifestSha256.Length != 64
            || !settings.FiscalDecisionManifestSha256.All(Uri.IsHexDigit))
            errors.Add("Debe vincular el SHA-256 del expediente DEC-01–DEC-15 validado.");
        if (string.IsNullOrWhiteSpace(settings.FiscalEvidenceReference)
            || settings.FiscalEvidenceReference.Trim().Length < 5)
            errors.Add("Debe indicar la referencia externa del expediente fiscal aprobado.");
        return errors;
    }

    public static FiscalProfileRevision CreateRevision(BusinessSettings settings, Guid actorId)
    {
        if (!settings.FiscalValidFrom.HasValue
            || !settings.FiscalApprovedAt.HasValue
            || !settings.FiscalApprovedByUserId.HasValue
            || string.IsNullOrWhiteSpace(settings.FiscalDecisionManifestSha256)
            || string.IsNullOrWhiteSpace(settings.FiscalEvidenceReference))
        {
            throw new InvalidOperationException("El perfil fiscal no contiene los datos necesarios para crear su revisión.");
        }

        var snapshot = new
        {
            schemaVersion = 1,
            settings.BusinessName,
            settings.RTN,
            settings.Address,
            settings.Phone,
            settings.Email,
            settings.IsvRate,
            settings.TouristTaxRate,
            settings.FiscalProfileVersion,
            settings.FiscalValidFrom,
            settings.FiscalValidUntil,
            decisionManifestSha256 = settings.FiscalDecisionManifestSha256.ToLowerInvariant(),
            settings.FiscalEvidenceReference
        };
        var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return new FiscalProfileRevision
        {
            BusinessSettingsId = settings.Id,
            Version = settings.FiscalProfileVersion,
            Status = FiscalProfileStatus.Aprobado,
            ValidFrom = settings.FiscalValidFrom.Value,
            ValidUntil = settings.FiscalValidUntil,
            DecisionManifestSha256 = settings.FiscalDecisionManifestSha256.ToLowerInvariant(),
            EvidenceReference = settings.FiscalEvidenceReference.Trim(),
            SnapshotJson = json,
            SnapshotSha256 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json))).ToLowerInvariant(),
            ApprovedAt = settings.FiscalApprovedAt.Value,
            ApprovedByUserId = actorId,
            ApprovalNote = settings.FiscalApprovalNote ?? string.Empty
        };
    }
}
