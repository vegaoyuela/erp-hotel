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
    [Authorize]
    public class DocumentAuthorizationsController : ControllerBase
    {
        private readonly IDocumentAuthorizationRepository _repo;
        private readonly IMapper _mapper;
        private readonly AuthorizationAttachmentStore _attachmentStore;

        private readonly IUnitOfWork _unitOfWork;
        private readonly HotelListQueryService _lists;
        private readonly IHotelClock _clock;

        public DocumentAuthorizationsController(
            IDocumentAuthorizationRepository repo,
            IMapper mapper,
            AuthorizationAttachmentStore attachmentStore, IUnitOfWork unitOfWork, HotelListQueryService lists, IHotelClock clock)
        {
            _unitOfWork = unitOfWork;
            _lists = lists;
            _clock = clock;
            _repo = repo;
            _mapper = mapper;
            _attachmentStore = attachmentStore;
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
            if (!Enum.TryParse<InvoiceDocumentType>(request.DocumentType, out var type)
                || type is not (InvoiceDocumentType.Factura or InvoiceDocumentType.NotaCredito or InvoiceDocumentType.NotaDebito))
                return BadRequest("Tipo de documento fiscal inválido");

            var existing = await _repo.GetByCAIAsync(type, request.CAINumber);
            if (existing != null) return BadRequest("Ya existe esa autorización para el tipo de documento indicado");

            var authorization = new DocumentAuthorization
            {
                DocumentType = type,
                CAINumber = request.CAINumber,
                IssueDate = request.IssueDate,
                DueDate = request.DueDate,
                InitialRange = request.InitialRange,
                FinalRange = request.FinalRange,
                CurrentCorrelative = request.InitialRange,
                Status = CAIStatus.Activo
            };

            await _repo.AddAsync(authorization);
            await _unitOfWork.SaveChangesAsync(HttpContext.RequestAborted);
            return CreatedAtAction(nameof(GetById), new { id = authorization.Id }, _mapper.Map<DocumentAuthorizationDto>(authorization));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionNames.ManageTaxes)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(HttpContext.RequestAborted);
            return NoContent();
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
            if (!Enum.TryParse<InvoiceDocumentType>(request.DocumentType, out var type)
                || type is not (InvoiceDocumentType.Factura or InvoiceDocumentType.NotaCredito or InvoiceDocumentType.NotaDebito))
                return BadRequest("Tipo de documento fiscal inválido");

            var existing = await _repo.GetByCAIAsync(type, request.CAINumber);
            if (existing != null) return BadRequest("Ya existe esa autorización para el tipo de documento indicado");

            var authorization = new DocumentAuthorization
            {
                Id = Guid.NewGuid(),
                DocumentType = type,
                CAINumber = request.CAINumber,
                IssueDate = request.IssueDate,
                DueDate = request.DueDate,
                InitialRange = request.InitialRange,
                FinalRange = request.FinalRange,
                CurrentCorrelative = request.InitialRange,
                Status = CAIStatus.Activo
            };

            if (file is not null)
            {
                try
                {
                    authorization.AttachmentPath = await _attachmentStore.SavePdfAsync(
                        authorization.Id,
                        file,
                        cancellationToken);
                }
                catch (AttachmentValidationException exception)
                {
                    return Problem(
                        type: "https://httpstatuses.com/400",
                        title: "Adjunto rechazado",
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exception.Message);
                }
            }

            try
            {
                await _repo.AddAsync(authorization);
                await _unitOfWork.SaveChangesAsync(HttpContext.RequestAborted);
            }
            catch
            {
                _attachmentStore.DeleteIfExists(authorization.AttachmentPath);
                throw;
            }
            return CreatedAtAction(nameof(GetById), new { id = authorization.Id }, _mapper.Map<DocumentAuthorizationDto>(authorization));
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
