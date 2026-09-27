using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;

namespace hotel_erp.UnitTests;

public class FiscalProfileServiceTests
{
    [Fact]
    public void ValidateForApproval_RejectsEmptyProfile()
    {
        var errors = FiscalProfileService.ValidateForApproval(new BusinessSettings());

        Assert.Contains(errors, error => error.Contains("razón social", StringComparison.Ordinal));
        Assert.Contains(errors, error => error.Contains("RTN", StringComparison.Ordinal));
        Assert.Contains(errors, error => error.Contains("dirección", StringComparison.Ordinal));
    }

    [Fact]
    public void ValidateForApproval_AcceptsCompleteProfile()
    {
        var settings = new BusinessSettings
        {
            BusinessName = "Hotel de prueba",
            RTN = "08011999123456",
            Address = "Tegucigalpa",
            IsvRate = 0.15m,
            TouristTaxRate = 0.04m,
            FiscalValidFrom = new DateOnly(2026, 9, 27),
            FiscalDecisionManifestSha256 = new string('a', 64),
            FiscalEvidenceReference = "EXP-SINTETICO-001"
        };

        Assert.Empty(FiscalProfileService.ValidateForApproval(settings));
    }

    [Fact]
    public void CreateRevision_PreservesApprovedSnapshotAndHashesIt()
    {
        var actorId = Guid.NewGuid();
        var settings = new BusinessSettings
        {
            Id = Guid.NewGuid(),
            BusinessName = "Hotel de prueba",
            RTN = "08011999123456",
            Address = "Tegucigalpa",
            IsvRate = 0.15m,
            TouristTaxRate = 0.04m,
            FiscalProfileVersion = 3,
            FiscalProfileStatus = FiscalProfileStatus.Aprobado,
            FiscalValidFrom = new DateOnly(2026, 9, 27),
            FiscalDecisionManifestSha256 = new string('a', 64),
            FiscalEvidenceReference = "EXP-SINTETICO-001",
            FiscalApprovedAt = new DateTime(2026, 9, 27, 18, 0, 0, DateTimeKind.Utc),
            FiscalApprovedByUserId = actorId,
            FiscalApprovalNote = "Aprobación sintética automatizada"
        };

        var revision = FiscalProfileService.CreateRevision(settings, actorId);
        settings.BusinessName = "Nombre modificado después";

        Assert.Equal(3, revision.Version);
        Assert.Equal(new string('a', 64), revision.DecisionManifestSha256);
        Assert.Equal(64, revision.SnapshotSha256.Length);
        Assert.Contains("Hotel de prueba", revision.SnapshotJson, StringComparison.Ordinal);
        Assert.DoesNotContain("Nombre modificado después", revision.SnapshotJson, StringComparison.Ordinal);
    }
}
