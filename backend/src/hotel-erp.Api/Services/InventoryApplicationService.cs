using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Inventory;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record InventoryMutationResult(Guid Id);

public sealed class InventoryApplicationService(
    ApplicationDbContext context,
    IAuditService auditService,
    IHotelClock clock,
    IApplicationFailureInjector failureInjector)
{
    public async Task<ApplicationMutationResult<Category>> CreateCategoryAsync(
        Guid userId,
        CreateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return ApplicationMutationResult<Category>.BadRequest("El nombre es obligatorio");

        var entity = new Category { Name = request.Name.Trim(), Description = NullIfWhiteSpace(request.Description) };
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        context.Categories.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        await auditService.LogAsync(userId, "CreateInventoryCategory", nameof(Category), entity.Id, new { entity.Name });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<Category>.Success(entity);
    }

    public async Task<ApplicationMutationResult<InventoryMutationResult>> UpdateCategoryAsync(
        Guid id,
        Guid userId,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
            return ApplicationMutationResult<InventoryMutationResult>.BadRequest("El nombre no puede quedar vacío");

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var entity = await context.Categories.SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (entity is null)
            return ApplicationMutationResult<InventoryMutationResult>.NotFound("Categoría no encontrada");

        if (request.Name is not null) entity.Name = request.Name.Trim();
        if (request.Description is not null) entity.Description = NullIfWhiteSpace(request.Description);
        await context.SaveChangesAsync(cancellationToken);
        await auditService.LogAsync(userId, "UpdateInventoryCategory", nameof(Category), id, new { entity.Name });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<InventoryMutationResult>.Success(new(id));
    }

    public async Task<ApplicationMutationResult<InventoryMutationResult>> DeleteCategoryAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var entity = await context.Categories.SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (entity is null)
            return ApplicationMutationResult<InventoryMutationResult>.NotFound("Categoría no encontrada");
        if (await context.Products.AnyAsync(product => product.CategoryId == id, cancellationToken))
            return ApplicationMutationResult<InventoryMutationResult>.Conflict("No se puede eliminar una categoría con productos asociados");

        entity.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
        await auditService.LogAsync(userId, "DeleteInventoryCategory", nameof(Category), id, new { entity.Name });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<InventoryMutationResult>.Success(new(id));
    }

    public async Task<ApplicationMutationResult<Product>> CreateProductAsync(
        Guid userId,
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = ValidateProduct(request.Name, request.UnitPrice, request.CurrentStock, request.MinStockLevel);
        if (validation is not null)
            return ApplicationMutationResult<Product>.BadRequest(validation);
        if (request.CategoryId.HasValue && !await CategoryExistsAsync(request.CategoryId.Value, cancellationToken))
            return ApplicationMutationResult<Product>.BadRequest("Categoría no encontrada");

        var entity = new Product
        {
            Name = request.Name.Trim(),
            Description = NullIfWhiteSpace(request.Description),
            SKU = NullIfWhiteSpace(request.SKU),
            CategoryId = request.CategoryId,
            UnitPrice = TaxService.RoundCurrency(request.UnitPrice),
            CurrentStock = request.CurrentStock,
            MinStockLevel = request.MinStockLevel,
            IsActive = request.IsActive
        };
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        context.Products.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateInventoryProduct",
            nameof(Product),
            entity.Id,
            new { entity.Name, entity.SKU, entity.CurrentStock });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<Product>.Success(entity);
    }

    public async Task<ApplicationMutationResult<InventoryMutationResult>> UpdateProductAsync(
        Guid id,
        Guid userId,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
            return ApplicationMutationResult<InventoryMutationResult>.BadRequest("El nombre no puede quedar vacío");
        if (request.UnitPrice < 0m || request.CurrentStock < 0 || request.MinStockLevel < 0)
            return ApplicationMutationResult<InventoryMutationResult>.BadRequest("Precio y existencias no pueden ser negativos");
        if (request.CategoryId.HasValue && !await CategoryExistsAsync(request.CategoryId.Value, cancellationToken))
            return ApplicationMutationResult<InventoryMutationResult>.BadRequest("Categoría no encontrada");

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockProductAsync(id, cancellationToken);
        var entity = await context.Products.SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (entity is null)
            return ApplicationMutationResult<InventoryMutationResult>.NotFound("Producto no encontrado");

        if (request.Name is not null) entity.Name = request.Name.Trim();
        if (request.Description is not null) entity.Description = NullIfWhiteSpace(request.Description);
        if (request.SKU is not null) entity.SKU = NullIfWhiteSpace(request.SKU);
        if (request.CategoryId.HasValue) entity.CategoryId = request.CategoryId;
        if (request.UnitPrice.HasValue) entity.UnitPrice = TaxService.RoundCurrency(request.UnitPrice.Value);
        if (request.CurrentStock.HasValue) entity.CurrentStock = request.CurrentStock.Value;
        if (request.MinStockLevel.HasValue) entity.MinStockLevel = request.MinStockLevel.Value;
        if (request.IsActive.HasValue) entity.IsActive = request.IsActive.Value;
        await context.SaveChangesAsync(cancellationToken);
        await auditService.LogAsync(
            userId,
            "UpdateInventoryProduct",
            nameof(Product),
            id,
            new { entity.Name, entity.SKU, entity.CurrentStock });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<InventoryMutationResult>.Success(new(id));
    }

    public async Task<ApplicationMutationResult<InventoryMutationResult>> DeleteProductAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockProductAsync(id, cancellationToken);
        var entity = await context.Products.SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (entity is null)
            return ApplicationMutationResult<InventoryMutationResult>.NotFound("Producto no encontrado");

        entity.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
        await auditService.LogAsync(userId, "DeleteInventoryProduct", nameof(Product), id, new { entity.Name, entity.SKU });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<InventoryMutationResult>.Success(new(id));
    }

    public async Task<ApplicationMutationResult<InventoryMovement>> CreateMovementAsync(
        Guid userId,
        CreateInventoryMovementRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<InventoryMovementType>(request.MovementType, true, out var movementType))
            return ApplicationMutationResult<InventoryMovement>.BadRequest("Tipo de movimiento inválido");
        if (movementType == InventoryMovementType.Ajuste)
        {
            if (!request.TargetStock.HasValue || request.TargetStock.Value < 0)
                return ApplicationMutationResult<InventoryMovement>.BadRequest("El ajuste requiere una existencia objetivo no negativa");
        }
        else if (request.Quantity <= 0)
        {
            return ApplicationMutationResult<InventoryMovement>.BadRequest("La cantidad debe ser mayor que cero");
        }
        if (request.UnitPrice < 0m)
            return ApplicationMutationResult<InventoryMovement>.BadRequest("El precio unitario no puede ser negativo");

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await LockProductAsync(request.ProductId, cancellationToken);
        var product = await context.Products
            .SingleOrDefaultAsync(candidate => candidate.Id == request.ProductId, cancellationToken);
        if (product is null || !product.IsActive)
            return ApplicationMutationResult<InventoryMovement>.NotFound("Producto no encontrado o inactivo");

        var currentStock = product.CurrentStock;
        var newStock = movementType switch
        {
            InventoryMovementType.Entrada or InventoryMovementType.Compra => currentStock + request.Quantity,
            InventoryMovementType.Salida or InventoryMovementType.Venta => currentStock - request.Quantity,
            InventoryMovementType.Ajuste => request.TargetStock!.Value,
            _ => currentStock
        };
        if (newStock < 0)
            return ApplicationMutationResult<InventoryMovement>.Conflict("El inventario no puede quedar negativo");

        var effectiveQuantity = movementType == InventoryMovementType.Ajuste
            ? Math.Abs(newStock - currentStock)
            : request.Quantity;
        product.CurrentStock = newStock;
        decimal? unitPrice = request.UnitPrice.HasValue
            ? TaxService.RoundCurrency(request.UnitPrice.Value)
            : null;
        var movement = new InventoryMovement
        {
            ProductId = product.Id,
            Product = product,
            MovementType = movementType,
            Quantity = effectiveQuantity,
            UnitPrice = unitPrice,
            TotalValue = unitPrice.HasValue ? TaxService.RoundCurrency(unitPrice.Value * effectiveQuantity) : null,
            NewStock = newStock,
            ReferenceId = request.ReferenceId,
            UserId = userId,
            MovementDate = clock.HondurasNow
        };
        context.InventoryMovements.Add(movement);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.InventoryMovementAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateInventoryMovement",
            nameof(InventoryMovement),
            movement.Id,
            new
            {
                movement.ProductId,
                MovementType = movement.MovementType.ToString(),
                movement.Quantity,
                PreviousStock = currentStock,
                movement.NewStock,
                movement.ReferenceId
            });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<InventoryMovement>.Success(movement);
    }

    private Task<bool> CategoryExistsAsync(Guid id, CancellationToken cancellationToken)
        => context.Categories.AnyAsync(category => category.Id == id, cancellationToken);

    private async Task LockProductAsync(Guid id, CancellationToken cancellationToken)
    {
        var lockName = $"inventory-product:{id:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({lockName}));",
            cancellationToken);
    }

    private static string? ValidateProduct(string name, decimal unitPrice, int currentStock, int minStockLevel)
    {
        if (string.IsNullOrWhiteSpace(name)) return "El nombre es obligatorio";
        if (unitPrice < 0m || currentStock < 0 || minStockLevel < 0)
            return "Precio y existencias no pueden ser negativos";
        return null;
    }

    private static string? NullIfWhiteSpace(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
