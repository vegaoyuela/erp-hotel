using hotel_erp.Api.Database.Entities;

namespace hotel_erp.Api.Database.Entities
{
    public class BusinessSettings : BaseEntity
    {
        public string BusinessName { get; set; } = string.Empty;
        public string RTN { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? LogoBase64 { get; set; }
        public string Footer { get; set; } = "¡Gracias por su preferencia!";
        public decimal IsvRate { get; set; }
        public decimal TouristTaxRate { get; set; }
        public FiscalProfileStatus FiscalProfileStatus { get; set; } = FiscalProfileStatus.Borrador;
        public int FiscalProfileVersion { get; set; } = 1;
        public DateOnly? FiscalValidFrom { get; set; }
        public DateOnly? FiscalValidUntil { get; set; }
        public DateTime? FiscalApprovedAt { get; set; }
        public Guid? FiscalApprovedByUserId { get; set; }
        public string? FiscalApprovalNote { get; set; }
        public DateTime? FiscalRetiredAt { get; set; }
        public Guid? FiscalRetiredByUserId { get; set; }
        public string? FiscalRetirementReason { get; set; }
        public string? FiscalDecisionManifestSha256 { get; set; }
        public string? FiscalEvidenceReference { get; set; }
        public string? PrintPrinterName { get; set; }
        public int PrintWidth { get; set; } = 46;
        public int PrintLogoHeight { get; set; } = 40;
        public string PrintFontSize { get; set; } = "condensed";
        public int PrintLineSpacing { get; set; } = 1;
        // Secciones visibles
        public bool ShowLogo { get; set; } = true;
        public bool ShowHeader { get; set; } = true;
        public bool ShowFiscal { get; set; }
        public bool ShowGuest { get; set; } = true;
        public bool ShowItems { get; set; } = true;
        public bool ShowTotals { get; set; } = true;
        public bool ShowPayment { get; set; } = true;
        public bool ShowFooter { get; set; } = true;
        // Estilo
        public string HeaderAlign { get; set; } = "center";
        public string SeparatorChar { get; set; } = "-";
        public int MarginLeft { get; set; } = 0;
    }

    public enum FiscalProfileStatus
    {
        Borrador,
        Aprobado,
        Retirado
    }

    public class FiscalProfileRevision : BaseEntity
    {
        public Guid BusinessSettingsId { get; set; }
        public int Version { get; set; }
        public FiscalProfileStatus Status { get; set; }
        public DateOnly ValidFrom { get; set; }
        public DateOnly? ValidUntil { get; set; }
        public string DecisionManifestSha256 { get; set; } = string.Empty;
        public string EvidenceReference { get; set; } = string.Empty;
        public string SnapshotJson { get; set; } = string.Empty;
        public string SnapshotSha256 { get; set; } = string.Empty;
        public DateTime ApprovedAt { get; set; }
        public Guid ApprovedByUserId { get; set; }
        public string ApprovalNote { get; set; } = string.Empty;
        public DateTime? RetiredAt { get; set; }
        public Guid? RetiredByUserId { get; set; }
        public string? RetirementReason { get; set; }
    }
}
