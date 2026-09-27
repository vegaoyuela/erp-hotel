using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;

namespace hotel_erp.Api.Infrastructure;

public sealed class ApiProblemDetailsFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var statusCode = ResolveStatusCode(context.Result);
        if (statusCode is null || statusCode < 400)
        {
            await next();
            return;
        }

        var source = (context.Result as ObjectResult)?.Value;
        var problem = source as ProblemDetails ?? CreateProblem(context, statusCode.Value, source);
        CompleteProblem(problem, context, statusCode.Value);

        var result = new ObjectResult(problem)
        {
            StatusCode = statusCode,
            DeclaredType = problem.GetType()
        };
        result.ContentTypes.Add("application/problem+json");
        context.Result = result;
        await next();
    }

    private static int? ResolveStatusCode(IActionResult result) => result switch
    {
        ObjectResult objectResult => objectResult.StatusCode,
        StatusCodeResult statusCodeResult => statusCodeResult.StatusCode,
        _ => null
    };

    private static ProblemDetails CreateProblem(ResultExecutingContext context, int statusCode, object? source)
    {
        var detail = ExtractProperty(source, "message") as string
            ?? (source as string)
            ?? DefaultDetail(statusCode);
        var problem = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = DefaultTitle(statusCode),
            Status = statusCode,
            Detail = detail,
            Instance = context.HttpContext.Request.Path
        };

        var errors = ExtractProperty(source, "errors");
        if (errors is not null)
            problem.Extensions["errors"] = errors;
        return problem;
    }

    private static void CompleteProblem(
        ProblemDetails problem,
        ResultExecutingContext context,
        int statusCode)
    {
        problem.Status ??= statusCode;
        problem.Type ??= $"https://httpstatuses.com/{statusCode}";
        problem.Title ??= DefaultTitle(statusCode);
        problem.Detail ??= DefaultDetail(statusCode);
        problem.Instance ??= context.HttpContext.Request.Path;
        problem.Extensions.TryAdd("code", $"HTTP_{statusCode}");
        problem.Extensions.TryAdd("traceId", context.HttpContext.TraceIdentifier);
    }

    private static object? ExtractProperty(object? source, string name)
    {
        if (source is null) return null;
        if (source is IDictionary<string, object?> dictionary)
            return dictionary.TryGetValue(name, out var value) ? value : null;
        return source.GetType()
            .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
            ?.GetValue(source);
    }

    private static string DefaultTitle(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Solicitud inválida",
        StatusCodes.Status401Unauthorized => "Autenticación requerida",
        StatusCodes.Status403Forbidden => "Acceso denegado",
        StatusCodes.Status404NotFound => "Recurso no encontrado",
        StatusCodes.Status409Conflict => "Conflicto de operación",
        StatusCodes.Status422UnprocessableEntity => "Operación no procesable",
        StatusCodes.Status429TooManyRequests => "Demasiadas solicitudes",
        _ => ReasonPhrases.GetReasonPhrase(statusCode)
    };

    private static string DefaultDetail(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Revise los datos enviados.",
        StatusCodes.Status401Unauthorized => "Inicie sesión para continuar.",
        StatusCodes.Status403Forbidden => "Su usuario no tiene permiso para realizar esta operación.",
        StatusCodes.Status404NotFound => "El recurso solicitado no existe.",
        StatusCodes.Status409Conflict => "La operación entra en conflicto con el estado actual.",
        StatusCodes.Status422UnprocessableEntity => "Los datos no permiten completar la operación.",
        StatusCodes.Status429TooManyRequests => "Espere antes de volver a intentar esta operación.",
        _ => "La solicitud no pudo completarse."
    };
}
