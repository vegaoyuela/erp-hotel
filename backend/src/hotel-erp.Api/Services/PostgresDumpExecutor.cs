using Npgsql;

namespace hotel_erp.Api.Services;

public interface IPostgresDumpExecutor
{
    Task DumpAsync(string outputPath, string connectionString, CancellationToken cancellationToken = default);
}

public sealed class PostgresDumpExecutor : IPostgresDumpExecutor
{
    public async Task DumpAsync(
        string outputPath,
        string connectionString,
        CancellationToken cancellationToken = default)
    {
        var connection = new NpgsqlConnectionStringBuilder(connectionString);
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "pg_dump",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        startInfo.ArgumentList.Add("-h");
        startInfo.ArgumentList.Add(string.IsNullOrWhiteSpace(connection.Host) ? "localhost" : connection.Host);
        startInfo.ArgumentList.Add("-p");
        startInfo.ArgumentList.Add((connection.Port > 0 ? connection.Port : 5432).ToString());
        startInfo.ArgumentList.Add("-U");
        startInfo.ArgumentList.Add(connection.Username ?? "postgres");
        startInfo.ArgumentList.Add("-d");
        startInfo.ArgumentList.Add(connection.Database ?? "hotel_erp");
        startInfo.ArgumentList.Add("-F");
        startInfo.ArgumentList.Add("p");
        startInfo.ArgumentList.Add("-f");
        startInfo.ArgumentList.Add(outputPath);

        if (!string.IsNullOrEmpty(connection.Password))
            startInfo.Environment["PGPASSWORD"] = connection.Password;

        using var process = System.Diagnostics.Process.Start(startInfo)
            ?? throw new InvalidOperationException(
                "No se pudo iniciar pg_dump. Verifique su instalación y la variable PATH.");
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
            throw new InvalidOperationException($"pg_dump falló con código {process.ExitCode}: {error.Trim()}");
    }
}
