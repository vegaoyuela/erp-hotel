using hotel_erp.Api.Infrastructure;
using hotel_erp.Api.Services;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = PermissionNames.ManageAccounting)]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierApplicationService _application;
        private readonly ISupplierRepository _repo;
        private readonly IMapper _mapper;


        public SuppliersController(ISupplierRepository repo, IMapper mapper, SupplierApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll([FromQuery] string? search)
        {
            if (!string.IsNullOrEmpty(search))
                return Ok(_mapper.Map<IEnumerable<SupplierDto>>(await _repo.SearchAsync(search)));
            return Ok(_mapper.Map<IEnumerable<SupplierDto>>(await _repo.GetAllAsync()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetById(Guid id)
        {
            var supplier = await _repo.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(_mapper.Map<SupplierDto>(supplier));
        }

        [HttpPost]
        [ProducesResponseType(typeof(SupplierDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateSupplierRequest request)
        {
            return this.CatalogResult(await _application.UpdateAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return this.CatalogResult(await _application.DeleteAsync(SecurityActor.From(User), id, HttpContext.RequestAborted));
        }
    }
}

