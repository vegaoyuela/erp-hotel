using hotel_erp.Api.Infrastructure;
using hotel_erp.Api.Services;
using hotel_erp.Api.Dtos.Discount;
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
    [Authorize(Policy = HotelAccessPolicies.ReadDiscounts)]
    public class DiscountsController : ControllerBase
    {
        private readonly DiscountApplicationService _application;
        private readonly IDiscountRepository _repo;
        private readonly IMapper _mapper;


        public DiscountsController(IDiscountRepository repo, IMapper mapper, DiscountApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountDto>>> GetAll()
            => Ok(_mapper.Map<IEnumerable<DiscountDto>>(await _repo.GetAllAsync()));

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDto>> GetById(Guid id)
        {
            var discount = await _repo.GetByIdAsync(id);
            if (discount == null) return NotFound();
            return Ok(_mapper.Map<DiscountDto>(discount));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(typeof(DiscountDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<DiscountDto>> Create([FromBody] CreateDiscountRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateDiscountRequest request)
        {
            return this.CatalogResult(await _application.UpdateAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionNames.ManageSettings)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return this.CatalogResult(await _application.DeleteAsync(SecurityActor.From(User), id, HttpContext.RequestAborted));
        }
    }
}

