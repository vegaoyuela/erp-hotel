using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace hotel_erp.Api.Infrastructure;

public static class CatalogMutationResultExtensions
{
    public static ActionResult CatalogResult(this ControllerBase controller, CatalogMutationResult result, string? createdAction = null)
    {
        if (result.CreatedId is Guid id)
            return controller.CreatedAtAction(createdAction ?? throw new ArgumentNullException(nameof(createdAction)), new { id }, result.Value);
        if (result.Status == StatusCodes.Status204NoContent) return controller.NoContent();
        var value = result.Value is CatalogProblem problem
            ? new ProblemDetails { Type = problem.Type, Title = problem.Title, Status = problem.Status, Detail = problem.Detail }
            : result.Value;
        return controller.StatusCode(result.Status, value);
    }
}
