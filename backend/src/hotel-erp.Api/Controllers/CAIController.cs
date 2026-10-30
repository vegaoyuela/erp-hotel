using hotel_erp.Api.Infrastructure;
using hotel_erp.Api.Services;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = HotelAccessPolicies.ReadFiscalOptions)]
    public class CAIController : ControllerBase
    {
        private readonly CaiApplicationService _application;
        private readonly ICAIRepository _repo;
        private readonly IMapper _mapper;


        public CAIController(ICAIRepository repo, IMapper mapper, CaiApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CAIDto>>> GetAll()
            => Ok(_mapper.Map<IEnumerable<CAIDto>>(await _repo.GetAllAsync()));

        [HttpGet("active")]
        public async Task<ActionResult<CAIDto>> GetActive()
        {
            var cai = await _repo.GetActiveCAIAsync();
            if (cai == null) return NotFound("No hay un CAI activo");
            return Ok(_mapper.Map<CAIDto>(cai));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CAIDto>> GetById(Guid id)
        {
            var cai = await _repo.GetByIdAsync(id);
            if (cai == null) return NotFound();
            return Ok(_mapper.Map<CAIDto>(cai));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [ProducesResponseType(typeof(CAIDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<CAIDto>> Create([FromBody] CreateCAIRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
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
