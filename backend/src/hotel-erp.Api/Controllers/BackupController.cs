using hotel_erp.Api.Infrastructure;
using hotel_erp.Api.Database;
using hotel_erp.Api.Dtos.Common;
using System.ComponentModel.DataAnnotations;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = PermissionNames.ManageBackups)]
    public class BackupController : ControllerBase
    {
        private readonly IDatabaseBackupService _backupService;
        private readonly ApplicationDbContext _context;
        private readonly HotelListQueryService _lists;

        public BackupController(IDatabaseBackupService backupService, ApplicationDbContext context, HotelListQueryService lists)
        {
            _backupService = backupService;
            _context = context;
            _lists = lists;
        }

        [HttpPost("manual")]
        [ApiFileResponse("application/octet-stream")]
        public async Task<IActionResult> CreateManualBackup()
        {
            var last = await _backupService.RunLocalBackupAsync(HttpContext.RequestAborted);

            if (last.Status == hotel_erp.Api.Database.Entities.BackupStatus.Failed)
                return BadRequest(new { message = "Error al crear el respaldo" });

            if (!System.IO.File.Exists(last.LocalPath))
                return NotFound(new { message = "Archivo de respaldo no encontrado en disco" });

            var stream = System.IO.File.OpenRead(last.LocalPath);
            return File(stream, "application/octet-stream", last.FileName);
        }

        [HttpGet("logs/paged")]
        public async Task<ActionResult<PagedResult<BackupLogDto>>> GetLogsPage([FromQuery] BackupPageQuery query)
            => Ok(await _lists.BackupLogsAsync(query, HttpContext.RequestAborted));

        [HttpGet("logs")]
        public async Task<ActionResult<IEnumerable<BackupLogDto>>> GetLogs([FromQuery, Range(1, 100)] int limit = 20)
        {
            var logs = await _context.BackupLogs
                .OrderByDescending(b => b.StartedAt)
                .ThenByDescending(b => b.Id)
                .Take(limit)
                .Select(b => new BackupLogDto
                {
                    Id = b.Id,
                    StartedAt = b.StartedAt,
                    CompletedAt = b.CompletedAt,
                    FileName = b.FileName,
                    SizeBytes = b.SizeBytes,
                    Sha256Hash = b.Sha256Hash,
                    Status = b.Status.ToString(),
                    UploadedAt = b.UploadedAt,
                    UploadAttempts = b.UploadAttempts,
                    ErrorMessage = b.ErrorMessage
                })
                .ToListAsync();

            return Ok(logs);
        }
    }
}
