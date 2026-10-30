using hotel_erp.Api.Infrastructure;
using hotel_erp.Api.Dtos.Customer;
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
    [Authorize(Policy = HotelAccessPolicies.ReadGuests)]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerApplicationService _application;
        private readonly ICustomerRepository _repo;
        private readonly IMapper _mapper;

        private readonly HotelListQueryService _lists;

        public CustomersController(ICustomerRepository repo, IMapper mapper, HotelListQueryService lists, CustomerApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _lists = lists;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll([FromQuery] string? search)
        {
            if (!string.IsNullOrEmpty(search))
                return Ok(_mapper.Map<IEnumerable<CustomerDto>>(await _repo.SearchAsync(search)));
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(await _repo.GetAllAsync()));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<CustomerDto>>> GetPage([FromQuery] SearchPageQuery query)
            => Ok(await _lists.CustomersAsync(query, HttpContext.RequestAborted));

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id)
        {
            var customer = await _repo.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            return this.CatalogResult(await _application.UpdateAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return this.CatalogResult(await _application.DeleteAsync(SecurityActor.From(User), id, HttpContext.RequestAborted));
        }
    }
}


