using System.Security.Cryptography;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace hotel_erp.Api.Services;

public interface IDatabaseBackupService
{
    Task<BackupLog> RunLocalBackupAsync(CancellationToken cancellationToken = default);
    Task SyncPendingUploadsAsync(CancellationToken cancellationToken = default);
}

public sealed class DatabaseBackupService(
    ApplicationDbContext context,
    IConfiguration configuration,
    IOptions<BackupOptions> options,
    IHotelClock clock,
    IPostgresDumpExecutor dumpExecutor,
    ILogger<DatabaseBackupService> logger) : IDatabaseBackupService
{
    private readonly BackupOptions _options = options.Value;

    public async Task<BackupLog> RunLocalBackupAsync(CancellationToken cancellationToken = default)
    {
        var directory = Path.GetFullPath(_options.LocalPath);
        Directory.CreateDirectory(directory);
        var fileName = $"hotel_erp_{clock.UtcNow:yyyyMMdd_HHmmss_fff}_{Guid.NewGuid():N}.sql";
        var fullPath = Path.Combine(directory, fileName);
        var log = new BackupLog
        {
            StartedAt = clock.UtcNow,
            LocalPath = fullPath,
            FileName = fileName,
            Status = BackupStatus.CreatedLocal
        };
        context.BackupLogs.Add(log);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("No hay cadena de conexión DefaultConnection configurada");
            await dumpExecutor.DumpAsync(fullPath, connectionString, cancellationToken);
            var info = new FileInfo(fullPath);
            if (!info.Exists || info.Length == 0)
                throw new InvalidOperationException("pg_dump no produjo un archivo de respaldo válido");

            log.SizeBytes = info.Length;
            log.Sha256Hash = await ComputeSha256Async(fullPath, cancellationToken);
            log.CompletedAt = clock.UtcNow;
            log.Status = BackupStatus.PendingUpload;
            log.ErrorMessage = null;
        }
        catch (OperationCanceledException)
        {
            log.Status = BackupStatus.Failed;
            log.ErrorMessage = "La creación del respaldo fue cancelada";
            log.CompletedAt = clock.UtcNow;
            DeletePartialFile(fullPath);
            await context.SaveChangesAsync(CancellationToken.None);
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Database backup {BackupLogId} failed", log.Id);
            log.Status = BackupStatus.Failed;
            log.ErrorMessage = exception.Message;
            log.CompletedAt = clock.UtcNow;
            DeletePartialFile(fullPath);
        }

        await context.SaveChangesAsync(cancellationToken);
        return log;
    }

    public async Task SyncPendingUploadsAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.GoogleDriveUploadCommand)) return;

        var pending = await context.BackupLogs
            .Where(backup => backup.Status == BackupStatus.PendingUpload
                || backup.Status == BackupStatus.UploadFailed)
            .OrderBy(backup => backup.StartedAt)
            .Take(5)
            .ToListAsync(cancellationToken);

        foreach (var backup in pending)
        {
            if (!File.Exists(backup.LocalPath))
            {
                backup.Status = BackupStatus.UploadFailed;
                backup.ErrorMessage = "El archivo local del respaldo no existe";
                continue;
            }

            backup.UploadAttempts++;
            try
            {
                await ExecuteUploadCommandAsync(backup.LocalPath, cancellationToken);
                backup.Status = BackupStatus.UploadedToDrive;
                backup.UploadedAt = clock.UtcNow;
                backup.ErrorMessage = null;
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                logger.LogError(exception, "Backup upload {BackupLogId} failed", backup.Id);
                backup.Status = BackupStatus.UploadFailed;
                backup.ErrorMessage = exception.Message;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task ExecuteUploadCommandAsync(string filePath, CancellationToken cancellationToken)
    {
        var command = _options.GoogleDriveUploadCommand!.Replace("{file}", filePath);
        var parts = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            throw new InvalidOperationException("El comando de subida de respaldos está vacío");
        await ExecuteProcessAsync(parts[0], parts.Length > 1 ? parts[1] : string.Empty, cancellationToken);
    }

    private static async Task ExecuteProcessAsync(
        string fileName,
        string arguments,
        CancellationToken cancellationToken)
    {
        using var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fileName, arguments)
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }) ?? throw new InvalidOperationException($"No se pudo iniciar {fileName}");

        var standardError = process.StandardError.ReadToEndAsync();
        var standardOutput = process.StandardOutput.ReadToEndAsync();
        try
        {
            await process.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited) process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(CancellationToken.None);
            throw;
        }
        var error = await standardError;
        await standardOutput;
        if (process.ExitCode != 0)
            throw new InvalidOperationException(error.Trim());
    }

    private static async Task<string> ComputeSha256Async(
        string filePath,
        CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(filePath);
        var hash = await SHA256.HashDataAsync(stream, cancellationToken);
        return Convert.ToHexString(hash);
    }

    private static void DeletePartialFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath)) File.Delete(filePath);
        }
        catch (Exception)
        {
            // El intento fallido queda registrado; mantenimiento puede retirar el archivo parcial.
        }
    }
}
