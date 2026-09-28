using System.Security.Claims;
using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Inventory;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PermissionNames.ManageInventory)]
public class InventoryController(
    ApplicationDbContext context,
    InventoryApplicationService inventoryService) : ControllerBase
{
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        => Ok(await context.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .ToListAsync(HttpContext.RequestAborted));

    [HttpPost("categories")]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var result = await inventoryService.CreateCategoryAsync(
            CurrentUserId(),
            request,
            HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        return Ok(new CategoryDto
        {
            Id = result.Value!.Id,
            Name = result.Value.Name,
            Description = result.Value.Description
        });
    }

    [HttpPut("categories/{id}")]
    public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        var result = await inventoryService.UpdateCategoryAsync(
            id,
            CurrentUserId(),
            request,
            HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    [HttpDelete("categories/{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var result = await inventoryService.DeleteCategoryAsync(id, CurrentUserId(), HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] string? search)
    {
        var query = context.Products.AsNoTracking().Include(product => product.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(product => product.Name.Contains(search)
                || (product.SKU != null && product.SKU.Contains(search))
                || (product.Category != null && product.Category.Name.Contains(search)));
        }

        return Ok(await query
            .OrderBy(product => product.Name)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                CategoryId = product.CategoryId,
                CategoryName = product.Category != null ? product.Category.Name : null,
                UnitPrice = product.UnitPrice,
                CurrentStock = product.CurrentStock,
                MinStockLevel = product.MinStockLevel,
                IsActive = product.IsActive
            })
            .ToListAsync(HttpContext.RequestAborted));
    }

    [HttpPost("products")]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var result = await inventoryService.CreateProductAsync(
            CurrentUserId(),
            request,
            HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        return Ok(await MapProductAsync(result.Value!.Id));
    }

    [HttpPut("products/{id}")]
    public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request)
    {
        var result = await inventoryService.UpdateProductAsync(
            id,
            CurrentUserId(),
            request,
            HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    [HttpDelete("products/{id}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        var result = await inventoryService.DeleteProductAsync(id, CurrentUserId(), HttpContext.RequestAborted);
        return result.Succeeded ? NoContent() : Failure(result);
    }

    [HttpGet("movements")]
    public async Task<ActionResult<IEnumerable<InventoryMovementDto>>> GetMovements([FromQuery] Guid? productId)
    {
        var query = context.InventoryMovements
            .AsNoTracking()
            .Include(movement => movement.Product)
            .Include(movement => movement.User)
            .AsQueryable();
        if (productId.HasValue)
            query = query.Where(movement => movement.ProductId == productId.Value);

        return Ok(await query
            .OrderByDescending(movement => movement.MovementDate)
            .ThenByDescending(movement => movement.CreatedAt)
            .Select(movement => ToMovementDto(movement))
            .ToListAsync(HttpContext.RequestAborted));
    }

    [HttpPost("movements")]
    public async Task<ActionResult<InventoryMovementDto>> CreateMovement(
        [FromBody] CreateInventoryMovementRequest request)
    {
        var result = await inventoryService.CreateMovementAsync(
            CurrentUserId(),
            request,
            HttpContext.RequestAborted);
        if (!result.Succeeded)
            return Failure(result);

        return Ok(await MapMovementAsync(result.Value!.Id));
    }

    private Task<ProductDto?> MapProductAsync(Guid id)
        => context.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                CategoryId = product.CategoryId,
                CategoryName = product.Category != null ? product.Category.Name : null,
                UnitPrice = product.UnitPrice,
                CurrentStock = product.CurrentStock,
                MinStockLevel = product.MinStockLevel,
                IsActive = product.IsActive
            })
            .FirstOrDefaultAsync(HttpContext.RequestAborted);

    private Task<InventoryMovementDto?> MapMovementAsync(Guid id)
        => context.InventoryMovements
            .AsNoTracking()
            .Include(movement => movement.Product)
            .Include(movement => movement.User)
            .Where(movement => movement.Id == id)
            .Select(movement => ToMovementDto(movement))
            .FirstOrDefaultAsync(HttpContext.RequestAborted);

    private static InventoryMovementDto ToMovementDto(InventoryMovement movement)
        => new()
        {
            Id = movement.Id,
            ProductId = movement.ProductId,
            ProductName = movement.Product.Name,
            MovementType = movement.MovementType.ToString(),
            Quantity = movement.Quantity,
            MovementDate = movement.MovementDate,
            UnitPrice = movement.UnitPrice,
            TotalValue = movement.TotalValue,
            NewStock = movement.NewStock,
            ReferenceId = movement.ReferenceId,
            UserName = movement.User != null ? movement.User.FirstName + " " + movement.User.LastName : null
        };

    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private ObjectResult Failure<T>(ApplicationMutationResult<T> result)
        where T : class
        => StatusCode(result.FailureStatus!.Value, result.FailureDetail);
}
