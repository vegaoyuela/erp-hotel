using hotel_erp.Api.Infrastructure;
using AutoMapper;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/document-authorizations")]
    [Authorize(Policy = HotelAccessPolicies.ReadFiscalOptions)]
    public class DocumentAuthorizationsController : ControllerBase
    {
        private readonly DocumentAuthorizationApplicationService _application;
        private readonly IDocumentAuthorizationRepository _repo;
        private readonly IMapper _mapper;
        private readonly AuthorizationAttachmentStore _attachmentStore;

        private readonly HotelListQueryService _lists;
        private readonly IHotelClock _clock;

        public DocumentAuthorizationsController(IDocumentAuthorizationRepository repo, IMapper mapper, AuthorizationAttachmentStore attachmentStore, HotelListQueryService lists, IHotelClock clock, DocumentAuthorizationApplicationService application)
        {
            _repo = repo;
            _mapper = mapper;
            _attachmentStore = attachmentStore;
            _lists = lists;
            _clock = clock;
            _application = application;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentAuthorizationDto>>> GetAll()
            => Ok(_mapper.Map<IEnumerable<DocumentAuthorizationDto>>(await _repo.GetAllAsync()));

        [HttpGet("options/paged")]
        public async Task<ActionResult<PagedResult<AuthorizationOptionDto>>> GetOptionsPage([FromQuery] AuthorizationPageQuery query)
            => Ok(await _lists.AuthorizationsAsync(query, _clock.Today, HttpContext.RequestAborted));

        [HttpGet("active/{documentType}")]
        public async Task<ActionResult<DocumentAuthorizationDto>> GetActive(string documentType)
        {
            if (!Enum.TryParse<InvoiceDocumentType>(documentType, out var type))
                return BadRequest("Tipo de documento fiscal inválido");

            var authorization = await _repo.GetActiveAsync(type);
            if (authorization == null) return NotFound($"No hay autorización activa para {documentType}");
            return Ok(_mapper.Map<DocumentAuthorizationDto>(authorization));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentAuthorizationDto>> GetById(Guid id)
        {
            var authorization = await _repo.GetByIdAsync(id);
            if (authorization == null) return NotFound();
            return Ok(_mapper.Map<DocumentAuthorizationDto>(authorization));
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [ProducesResponseType(typeof(DocumentAuthorizationDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<DocumentAuthorizationDto>> Create([FromBody] CreateDocumentAuthorizationRequest request)
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

        [HttpPost("with-file")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(DocumentAuthorizationDto), StatusCodes.Status201Created)]
        public async Task<ActionResult<DocumentAuthorizationDto>> CreateWithFile(
            [FromForm] CreateDocumentAuthorizationRequest request,
            IFormFile? file,
            CancellationToken cancellationToken)
        {
            return this.CatalogResult(await _application.CreateWithFileAsync(SecurityActor.From(User), request, file, cancellationToken), nameof(GetById));
        }

        [HttpGet("{id}/file")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [ApiFileResponse("application/pdf")]
        public async Task<ActionResult> GetFile(Guid id)
        {
            var authorization = await _repo.GetByIdAsync(id);
            if (authorization == null) return NotFound();
            var stream = _attachmentStore.OpenPdf(authorization.AttachmentPath);
            if (stream is null) return NotFound("No hay un PDF válido para esta autorización");

            Response.Headers.CacheControl = "no-store";
            Response.Headers.XContentTypeOptions = "nosniff";
            return File(
                stream,
                "application/pdf",
                $"autorizacion-fiscal-{authorization.Id:N}.pdf",
                enableRangeProcessing: false);
        }
    }
}
