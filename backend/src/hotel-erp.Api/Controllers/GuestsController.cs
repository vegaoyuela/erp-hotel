using hotel_erp.Api.Infrastructure;
using hotel_erp.Api.Services;
using hotel_erp.Api.Dtos.Customer;
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
    public class GuestsController : ControllerBase
    {
        private readonly GuestApplicationService _application;
        private readonly IGuestRepository _repo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IMapper _mapper;

        private readonly HotelListQueryService _lists;

        public GuestsController(IGuestRepository repo, IReservationRepository reservationRepo, IMapper mapper, HotelListQueryService lists, GuestApplicationService application)
        {
            _repo = repo;
            _reservationRepo = reservationRepo;
            _mapper = mapper;
            _lists = lists;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestDto>>> GetAll([FromQuery] string? search)
        {
            if (!string.IsNullOrEmpty(search))
                return Ok(_mapper.Map<IEnumerable<GuestDto>>(await _repo.SearchAsync(search)));
            return Ok(_mapper.Map<IEnumerable<GuestDto>>(await _repo.GetAllAsync()));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<GuestDto>>> GetPage([FromQuery] GuestPageQuery query)
            => Ok(await _lists.GuestsAsync(query, HttpContext.RequestAborted));

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestDto>> GetById(Guid id)
        {
            var guest = await _repo.GetByIdAsync(id);
            if (guest == null) return NotFound();
            return Ok(_mapper.Map<GuestDto>(guest));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(typeof(GuestDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<GuestDto>> Create([FromBody] CreateGuestRequest request)
        {
            return this.CatalogResult(await _application.CreateAsync(SecurityActor.From(User), request, HttpContext.RequestAborted), nameof(GetById));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateGuestRequest request)
        {
            return this.CatalogResult(await _application.UpdateAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted));
        }

        [HttpGet("{id}/stats")]
        public async Task<ActionResult<GuestStatsDto>> GetStats(Guid id)
        {
            var guest = await _repo.GetByIdAsync(id);
            if (guest == null) return NotFound();

            var reservations = await _reservationRepo.GetByGuestAsync(id);
            var completedVisits = reservations.Where(r => r.Status == hotel_erp.Api.Database.Entities.ReservationStatus.CheckOut).ToList();
            var lastVisit = completedVisits.OrderByDescending(r => r.CheckOutDate).FirstOrDefault();
            var totalVisits = completedVisits.Count;
            var isFrequent = totalVisits >= 2;

            return Ok(new GuestStatsDto
            {
                TotalVisits = totalVisits,
                Classification = isFrequent ? "Cliente Frecuente" : guest.Classification ?? "Normal",
                LastVisit = lastVisit?.CheckOutDate.ToString("dd/MM/yyyy"),
                IsFrequent = isFrequent
            });
        }

        [HttpPatch("{id}/classification")]
        [Authorize(Policy = PermissionNames.ManageReservations)]
        [ProducesResponseType(typeof(GuestClassificationDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateClassification(Guid id, [FromBody] UpdateClassificationRequest request)
        {
            return this.CatalogResult(await _application.UpdateClassificationAsync(SecurityActor.From(User), id, request, HttpContext.RequestAborted));
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


