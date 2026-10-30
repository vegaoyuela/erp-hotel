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
    [Route("api/room-types")]
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public class RoomTypesController : ControllerBase
    {
        private readonly RoomTypeApplicationService _application;
        private readonly IRoomTypeRepository _repo;
        private readonly IMapper _mapper;


        public RoomTypesController(IRoomTypeRepository repo, IMapper mapper, RoomTypeApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomTypeDto>>> GetAll()
            => Ok(_mapper.Map<IEnumerable<RoomTypeDto>>(await _repo.GetAllAsync()));

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTypeDto>> GetById(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<RoomTypeDto>(entity));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(typeof(RoomTypeDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<RoomTypeDto>> Create([FromBody] CreateRoomTypeRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateRoomTypeRequest request)
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

