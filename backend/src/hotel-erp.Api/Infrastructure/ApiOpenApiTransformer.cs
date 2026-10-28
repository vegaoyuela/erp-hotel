using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using hotel_erp.Api.Dtos.Common;

namespace hotel_erp.Api.Infrastructure;

public sealed class ApiOpenApiTransformer : IOpenApiOperationTransformer
{
    public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor action) return;
        operation.OperationId = $"{action.ControllerName}_{action.ActionName}";
        operation.Summary ??= $"{action.ControllerName}: {action.ActionName}";
        var metadata = action.EndpointMetadata;
        var authorized = !metadata.OfType<IAllowAnonymous>().Any() && metadata.OfType<IAuthorizeData>().Any();
        operation.Responses ??= new OpenApiResponses();
        var problem = await context.GetOrCreateSchemaAsync(typeof(ApiErrorDto), null, cancellationToken);
        void Error(string status, string description, IOpenApiSchema schema)
        {
            operation.Responses[status] = new OpenApiResponse
            {
                Description = description,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/problem+json"] = new() { Schema = schema }
                }
            };
        }
        Error("400", "Solicitud inválida; los errores de validación se indican por campo.", problem);
        Error("500", "Error interno normalizado; incluye traceId para seguimiento.", problem);
        if (authorized)
        {
            Error("401", "Se requiere una sesión válida.", problem);
            Error("403", "El operador carece de permisos o debe cambiar la contraseña.", problem);
            operation.Security ??= [];
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
            });
        }
        // Normalize only declared error responses; operation-specific statuses remain explicit.
        foreach (var entry in operation.Responses.Where(entry => int.TryParse(entry.Key, out var code) && code >= 400).ToList())
        {
            if (entry.Key != "400") Error(entry.Key, entry.Value.Description ?? "Error de operación.", problem);
        }
    }
}
