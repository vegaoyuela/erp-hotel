using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_erp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalProfileRevisionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FiscalDecisionManifestSha256",
                table: "BusinessSettings",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FiscalEvidenceReference",
                table: "BusinessSettings",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FiscalProfileRevisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BusinessSettingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidUntil = table.Column<DateOnly>(type: "date", nullable: true),
                    DecisionManifestSha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    EvidenceReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SnapshotJson = table.Column<string>(type: "text", nullable: false),
                    SnapshotSha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ApprovedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprovalNote = table.Column<string>(type: "text", nullable: false),
                    RetiredAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RetiredByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    RetirementReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalProfileRevisions", x => x.Id);
                    table.CheckConstraint("CK_FiscalProfileRevisions_DecisionHash", "\"DecisionManifestSha256\" ~ '^[0-9a-f]{64}$'");
                    table.CheckConstraint("CK_FiscalProfileRevisions_EvidenceReference", "length(btrim(\"EvidenceReference\")) >= 5");
                    table.CheckConstraint("CK_FiscalProfileRevisions_SnapshotHash", "\"SnapshotSha256\" ~ '^[0-9a-f]{64}$'");
                    table.CheckConstraint("CK_FiscalProfileRevisions_Status", "\"Status\" IN ('Aprobado', 'Retirado')");
                    table.CheckConstraint("CK_FiscalProfileRevisions_Validity", "\"ValidUntil\" IS NULL OR \"ValidUntil\" >= \"ValidFrom\"");
                    table.CheckConstraint("CK_FiscalProfileRevisions_Version", "\"Version\" > 0");
                    table.ForeignKey(
                        name: "FK_FiscalProfileRevisions_BusinessSettings_BusinessSettingsId",
                        column: x => x.BusinessSettingsId,
                        principalTable: "BusinessSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FiscalProfileRevisions_BusinessSettingsId_Version",
                table: "FiscalProfileRevisions",
                columns: new[] { "BusinessSettingsId", "Version" },
                unique: true);

            migrationBuilder.Sql("""
                UPDATE "BusinessSettings"
                SET "FiscalProfileStatus" = 'Borrador',
                    "FiscalProfileVersion" = "FiscalProfileVersion" + 1,
                    "FiscalApprovedAt" = NULL,
                    "FiscalApprovedByUserId" = NULL,
                    "FiscalApprovalNote" = NULL,
                    "FiscalRetiredAt" = NULL,
                    "FiscalRetiredByUserId" = NULL,
                    "FiscalRetirementReason" = NULL,
                    "FiscalDecisionManifestSha256" = NULL,
                    "FiscalEvidenceReference" = NULL
                WHERE "FiscalProfileStatus" <> 'Borrador';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FiscalProfileRevisions");

            migrationBuilder.DropColumn(
                name: "FiscalDecisionManifestSha256",
                table: "BusinessSettings");

            migrationBuilder.DropColumn(
                name: "FiscalEvidenceReference",
                table: "BusinessSettings");
        }
    }
}
