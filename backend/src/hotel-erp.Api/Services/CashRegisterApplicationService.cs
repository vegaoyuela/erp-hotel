using hotel_erp.Api.Services.Interfaces;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Cash;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed class CashRegisterApplicationService(
    ApplicationDbContext context,
    IAuditService auditService,
    IdempotencyService idempotencyService,
    IHotelClock clock,
    IApplicationFailureInjector failureInjector)
{
    public async Task<ApplicationMutationResult<CashRegisterDto>> CreateAsync(
        Guid userId,
        CreateCashRegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        const string catalogLock = "cash-register-catalog";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({catalogLock}));",
            cancellationToken);

        var name = request.Name.Trim();
        if (await context.CashRegisters.AnyAsync(
                register => register.Name.ToLower() == name.ToLower(),
                cancellationToken))
            return ApplicationMutationResult<CashRegisterDto>.Conflict("Ya existe una caja con ese nombre");

        var entity = new CashRegister
        {
            Name = name,
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
        };
        context.CashRegisters.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.CashRegisterCreateAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateCashRegister",
            nameof(CashRegister),
            entity.Id,
            new { entity.Name, entity.Description });
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<CashRegisterDto>.Success(ToDto(entity, null));
    }

    public async Task<ApplicationMutationResult<CashRegisterOperationDto>> OpenAsync(
        Guid id,
        Guid userId,
        OpenCashRegisterRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
        {
            return ApplicationMutationResult<CashRegisterOperationDto>.BadRequest(
                "Idempotency-Key debe ser un UUID válido");
        }

        var initialAmount = TaxService.RoundCurrency(request.InitialAmount);
        var requestHash = IdempotencyService.ComputeRequestHash(new
        {
            CashRegisterId = id,
            InitialAmount = initialAmount
        });
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        IdempotencyRecord? existingIntent;
        try
        {
            existingIntent = await idempotencyService.LockAndFindAsync(
                userId,
                "cash-register-open",
                normalizedKey,
                requestHash,
                cancellationToken);
        }
        catch (IdempotencyConflictException exception)
        {
            return ApplicationMutationResult<CashRegisterOperationDto>.Conflict(exception.Message);
        }

        if (existingIntent is not null)
        {
            var existingMovement = await context.CashMovements
                .SingleOrDefaultAsync(movement => movement.Id == existingIntent.ResourceId, cancellationToken);
            if (existingMovement is null || existingMovement.MovementType != CashMovementType.Apertura)
            {
                return ApplicationMutationResult<CashRegisterOperationDto>.Conflict(
                    "La operación idempotente no conserva su movimiento de apertura");
            }
            await transaction.CommitAsync(cancellationToken);
            return ApplicationMutationResult<CashRegisterOperationDto>.Success(OpenResult(existingMovement));
        }

        await LockCashRegisterAsync(id, cancellationToken);
        var register = await context.CashRegisters
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (register is null)
            return ApplicationMutationResult<CashRegisterOperationDto>.NotFound("Caja no encontrada");
        if (!register.IsActive)
            return ApplicationMutationResult<CashRegisterOperationDto>.Conflict("La caja está inactiva");

        var lastMovement = await LastMovementAsync(id, cancellationToken);
        if (lastMovement is not null && lastMovement.MovementType != CashMovementType.Cierre)
            return ApplicationMutationResult<CashRegisterOperationDto>.Conflict("La caja ya se encuentra abierta");

        var movement = new CashMovement
        {
            CashRegisterId = id,
            UserId = userId,
            MovementType = CashMovementType.Apertura,
            Amount = initialAmount,
            Description = "Apertura de caja",
            MovementDate = clock.HondurasNow,
            BalanceAfter = initialAmount
        };
        context.CashMovements.Add(movement);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.CashRegisterOpenAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "OpenCashRegister",
            nameof(CashRegister),
            register.Id,
            new { MovementId = movement.Id, InitialAmount = initialAmount });
        await idempotencyService.StoreAsync(
            userId,
            "cash-register-open",
            normalizedKey,
            requestHash,
            movement.Id,
            cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<CashRegisterOperationDto>.Success(OpenResult(movement));
    }

    public async Task<ApplicationMutationResult<CashRegisterOperationDto>> CloseAsync(
        Guid id,
        Guid userId,
        CloseCashRegisterRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
        {
            return ApplicationMutationResult<CashRegisterOperationDto>.BadRequest(
                "Idempotency-Key debe ser un UUID válido");
        }

        var countedAmount = TaxService.RoundCurrency(request.CountedAmount);
        var notes = request.Notes?.Trim();
        var requestHash = IdempotencyService.ComputeRequestHash(new
        {
            CashRegisterId = id,
            CountedAmount = countedAmount,
            Notes = notes
        });
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        IdempotencyRecord? existingIntent;
        try
        {
            existingIntent = await idempotencyService.LockAndFindAsync(
                userId,
                "cash-register-close",
                normalizedKey,
                requestHash,
                cancellationToken);
        }
        catch (IdempotencyConflictException exception)
        {
            return ApplicationMutationResult<CashRegisterOperationDto>.Conflict(exception.Message);
        }

        if (existingIntent is not null)
        {
            var existingMovement = await context.CashMovements
                .SingleOrDefaultAsync(movement => movement.Id == existingIntent.ResourceId, cancellationToken);
            if (existingMovement is null || existingMovement.MovementType != CashMovementType.Cierre)
            {
                return ApplicationMutationResult<CashRegisterOperationDto>.Conflict(
                    "La operación idempotente no conserva su movimiento de cierre");
            }
            await transaction.CommitAsync(cancellationToken);
            return ApplicationMutationResult<CashRegisterOperationDto>.Success(CloseResult(existingMovement));
        }

        await LockCashRegisterAsync(id, cancellationToken);
        var register = await context.CashRegisters
            .SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (register is null)
            return ApplicationMutationResult<CashRegisterOperationDto>.NotFound("Caja no encontrada");
        if (!register.IsActive)
            return ApplicationMutationResult<CashRegisterOperationDto>.Conflict("La caja está inactiva");

        var lastMovement = await LastMovementAsync(id, cancellationToken);
        if (lastMovement is null || lastMovement.MovementType == CashMovementType.Cierre)
            return ApplicationMutationResult<CashRegisterOperationDto>.Conflict("La caja ya se encuentra cerrada");

        var expectedAmount = TaxService.RoundCurrency(lastMovement.BalanceAfter);
        var difference = TaxService.RoundCurrency(countedAmount - expectedAmount);
        if (difference != 0m && (notes?.Length ?? 0) < 3)
        {
            return ApplicationMutationResult<CashRegisterOperationDto>.BadRequest(
                "Debe explicar la diferencia de caja con al menos 3 caracteres");
        }

        var movement = new CashMovement
        {
            CashRegisterId = id,
            UserId = userId,
            MovementType = CashMovementType.Cierre,
            Amount = 0m,
            Description = difference == 0m
                ? "Cierre de caja sin diferencia"
                : $"Cierre de caja. Diferencia: L {difference:0.00}",
            MovementDate = clock.HondurasNow,
            BalanceAfter = 0m,
            ExpectedAmount = expectedAmount,
            CountedAmount = countedAmount,
            Difference = difference,
            Notes = notes
        };
        context.CashMovements.Add(movement);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(
            ApplicationFailureCheckpoints.CashRegisterCloseAfterPersistence,
            cancellationToken);
        await auditService.LogAsync(
            userId,
            "CloseCashRegister",
            nameof(CashRegister),
            register.Id,
            new { MovementId = movement.Id, expectedAmount, countedAmount, difference, Notes = notes });
        await idempotencyService.StoreAsync(
            userId,
            "cash-register-close",
            normalizedKey,
            requestHash,
            movement.Id,
            cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return ApplicationMutationResult<CashRegisterOperationDto>.Success(CloseResult(movement));
    }

    public static CashRegisterDto ToDto(CashRegister register, CashMovement? latest) => new()
    {
        Id = register.Id,
        Name = register.Name,
        Description = register.Description,
        IsActive = register.IsActive,
        IsOpen = latest is not null && latest.MovementType != CashMovementType.Cierre,
        CurrentBalance = latest is not null && latest.MovementType != CashMovementType.Cierre
            ? latest.BalanceAfter
            : 0m,
        LastMovementDate = latest?.MovementDate
    };

    private async Task LockCashRegisterAsync(Guid id, CancellationToken cancellationToken)
    {
        var lockName = $"cash-register:{id:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({lockName}));",
            cancellationToken);
    }

    private Task<CashMovement?> LastMovementAsync(Guid id, CancellationToken cancellationToken)
        => context.CashMovements
            .Where(movement => movement.CashRegisterId == id)
            .OrderByDescending(movement => movement.CreatedAt)
            .ThenByDescending(movement => movement.Id)
            .FirstOrDefaultAsync(cancellationToken);

    private static CashRegisterOperationDto OpenResult(CashMovement movement) => new()
    {
        MovementId = movement.Id,
        Message = "Caja abierta exitosamente",
        CurrentBalance = movement.BalanceAfter
    };

    private static CashRegisterOperationDto CloseResult(CashMovement movement) => new()
    {
        MovementId = movement.Id,
        Message = "Caja cerrada exitosamente",
        CurrentBalance = 0m,
        ExpectedAmount = movement.ExpectedAmount,
        CountedAmount = movement.CountedAmount,
        Difference = movement.Difference
    };
}
