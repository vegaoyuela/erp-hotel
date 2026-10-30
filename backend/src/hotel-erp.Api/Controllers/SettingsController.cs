using hotel_erp.Api.Infrastructure;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;
using hotel_erp.Api.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/settings")]
    [Authorize(Policy = HotelAccessPolicies.ReadSettings)]
    public class SettingsController : ControllerBase
    {
        private readonly BusinessSettingsApplicationService _application;
        private readonly IBusinessSettingsRepository _repo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;


        public SettingsController(IBusinessSettingsRepository repo, IMapper mapper, ApplicationDbContext context, BusinessSettingsApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
            _application = application;
        }

        [HttpGet("business")]
        public async Task<ActionResult<BusinessSettingsDto>> Get()
        {
            var settings = await _repo.GetAsync() ?? new BusinessSettings();
            return Ok(_mapper.Map<BusinessSettingsDto>(settings));
        }

        [HttpGet("business/history")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        public async Task<ActionResult<IReadOnlyList<FiscalProfileRevisionDto>>> GetFiscalProfileHistory()
        {
            var revisions = await _context.FiscalProfileRevisions
                .AsNoTracking()
                .OrderByDescending(revision => revision.Version)
                .Select(revision => new FiscalProfileRevisionDto(
                    revision.Version,
                    revision.Status.ToString(),
                    revision.ValidFrom,
                    revision.ValidUntil,
                    revision.DecisionManifestSha256,
                    revision.EvidenceReference,
                    revision.SnapshotSha256,
                    revision.ApprovedAt,
                    revision.ApprovedByUserId,
                    revision.ApprovalNote,
                    revision.RetiredAt,
                    revision.RetiredByUserId,
                    revision.RetirementReason))
                .ToListAsync();
            return Ok(revisions);
        }

        [HttpPut("business")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        public async Task<ActionResult<BusinessSettingsDto>> Update([FromBody] UpdateBusinessSettingsRequest request)
        {
            return this.CatalogResult(await _application.UpdateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted));
        }

        [HttpPost("business/approve")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        public async Task<ActionResult<BusinessSettingsDto>> ApproveFiscalProfile([FromBody] ApproveFiscalProfileRequest request)
        {
            return this.CatalogResult(await _application.ApproveFiscalProfileAsync(SecurityActor.From(User), request, HttpContext.RequestAborted));
        }

        [HttpPost("business/retire")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        public async Task<ActionResult<BusinessSettingsDto>> RetireFiscalProfile([FromBody] RetireFiscalProfileRequest request)
        {
            return this.CatalogResult(await _application.RetireFiscalProfileAsync(SecurityActor.From(User), request, HttpContext.RequestAborted));
        }
    }
}
