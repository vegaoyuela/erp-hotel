using hotel_erp.Api.Infrastructure;
using hotel_erp.Api.Services;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/tax-configurations")]
    [Authorize(Policy = HotelAccessPolicies.ReadTaxRates)]
    public class TaxConfigurationsController : ControllerBase
    {
        private readonly TaxConfigurationApplicationService _application;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaxConfigurationsController(ApplicationDbContext context, IMapper mapper, TaxConfigurationApplicationService application)
        {
            _context = context;
            _mapper = mapper;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxConfigurationDto>>> GetAll()
        {
            var taxes = await _context.TaxConfigurations.OrderBy(t => t.Name).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TaxConfigurationDto>>(taxes));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [ProducesResponseType(typeof(TaxConfigurationDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<TaxConfigurationDto>> Create([FromBody] CreateTaxConfigurationRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetAll));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] CreateTaxConfigurationRequest request)
        {
            return this.CatalogResult(await _application.UpdateAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return this.CatalogResult(await _application.DeleteAsync(SecurityActor.From(User), id, HttpContext.RequestAborted));
        }
    }
}
