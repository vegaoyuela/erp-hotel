using System.Security.Claims;
using hotel_erp.Api.Services;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hotel_erp.Api.Infrastructure;

public sealed class OperationalAccessExceptionFilter(IAuditService audit) : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is not OperationalAccessException denied) return;
        if (denied.Status == StatusCodes.Status403Forbidden
            && Guid.TryParse(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
            await audit.LogAsync(id, "OperationalAccessDenied", "Endpoint", null,
                new { Method = context.HttpContext.Request.Method, Route = context.ActionDescriptor.AttributeRouteInfo?.Template });
        context.HttpContext.Response.Headers.CacheControl = "no-store";
        context.Result = new ObjectResult(new ProblemDetails
        {
            Status = denied.Status,
            Title = denied.Status == StatusCodes.Status401Unauthorized ? "Sesión revocada" : "Acceso denegado",
            Detail = denied.Message
        })
        { StatusCode = denied.Status };
        context.ExceptionHandled = true;
    }
}
