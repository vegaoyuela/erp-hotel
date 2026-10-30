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
    [Authorize(Policy = PermissionNames.ManageReservations)]
    public class RoomsController : ControllerBase
    {
        private readonly RoomApplicationService _application;
        private readonly IRoomRepository _repo;
        private readonly IMapper _mapper;


        public RoomsController(IRoomRepository repo, IMapper mapper, RoomApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAll()
            => Ok(_mapper.Map<IEnumerable<RoomDto>>(await _repo.GetAllAsync()));

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetById(Guid id)
        {
            var room = await _repo.GetByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(_mapper.Map<RoomDto>(room));
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAvailable([FromQuery] DateOnly checkIn, [FromQuery] DateOnly checkOut)
            => Ok(_mapper.Map<IEnumerable<RoomDto>>(await _repo.GetAvailableAsync(checkIn, checkOut)));

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetByStatus(string status)
            => Ok(_mapper.Map<IEnumerable<RoomDto>>(await _repo.GetByStatusAsync(status)));

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<RoomDto>> Create([FromBody] CreateRoomRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request)
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

