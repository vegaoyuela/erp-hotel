using System.Security.Claims;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Routing;

namespace hotel_erp.Api.Authorization;

public sealed class AuditedAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden && context.User.Identity?.IsAuthenticated == true
            && Guid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            var audit = context.RequestServices.GetRequiredService<IAuditService>();
            // Templates exclude IDs, query strings, bodies, cookies and credentials.
            var route = (context.GetEndpoint() as RouteEndpoint)?.RoutePattern.RawText;
            var requiredPolicies = context.GetEndpoint()?.Metadata.GetOrderedMetadata<IAuthorizeData>()
                .Select(data => data.Policy).Where(name => !string.IsNullOrWhiteSpace(name)).Distinct().ToArray();
            await audit.LogAsync(userId, "AccessDenied", "Endpoint", null,
                new { Method = context.Request.Method, Route = route, Policies = requiredPolicies });
        }
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
