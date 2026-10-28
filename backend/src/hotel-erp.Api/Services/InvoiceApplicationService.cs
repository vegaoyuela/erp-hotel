using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed record InvoiceMutationResult(
    Invoice? Invoice,
    int? FailureStatus,
    string? FailureDetail,
    string? FailureTitle = null,
    string? IdempotencyKey = null)
{
    public bool Succeeded => Invoice is not null;

    public static InvoiceMutationResult Success(Invoice invoice, string? idempotencyKey = null)
        => new(invoice, null, null, null, idempotencyKey);

    public static InvoiceMutationResult BadRequest(string detail, string? title = null)
        => new(null, StatusCodes.Status400BadRequest, detail, title);

    public static InvoiceMutationResult NotFound(string detail)
        => new(null, StatusCodes.Status404NotFound, detail);

    public static InvoiceMutationResult Conflict(string detail, string? title = null)
        => new(null, StatusCodes.Status409Conflict, detail, title);
}

public sealed class InvoiceApplicationService(
    ApplicationDbContext context,
    IInvoiceRepository repository,
    ICAIRepository caiRepository,
    ICaiCorrelativeService caiCorrelativeService,
    IAccountingService accountingService,
    IFiscalAuthorizationService fiscalAuthorizationService,
    FiscalProfileService fiscalProfileService,
    IdempotencyService idempotencyService,
    IAuditService auditService,
    IInvoiceCalculator invoiceCalculator,
    IHotelClock clock,
    IApplicationFailureInjector failureInjector)
{
    public async Task<InvoiceMutationResult> UpdateDraftAsync(
        Guid id,
        Guid userId,
        CreateInvoiceRequest request,
        CancellationToken cancellationToken = default)
    {
        var invoice = await repository.GetByIdAsync(id);
        if (invoice is null)
            return InvoiceMutationResult.NotFound("Factura no encontrada");
        if (invoice.DocumentType is InvoiceDocumentType.Factura or InvoiceDocumentType.NotaCredito or InvoiceDocumentType.NotaDebito)
            return InvoiceMutationResult.BadRequest("Un documento fiscal emitido no puede modificarse; use el documento de ajuste correspondiente");
        if (invoice.Status == InvoiceStatus.Anulada)
            return InvoiceMutationResult.BadRequest("No se puede modificar una factura anulada");

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        decimal subtotal = 0m;
        decimal isvAmount = 0m;
        decimal touristTax = 0m;
        decimal discounts = 0m;
        var items = request.Items.Select(item =>
        {
            var lineTotal = item.Quantity * item.UnitPrice;
            var discount = lineTotal * (item.DiscountPercentage / 100m);
            var lineAfterDiscount = lineTotal - discount;
            subtotal += lineAfterDiscount;
            if (!item.IsExempt)
                isvAmount += lineAfterDiscount * item.ISVRate;
            if (item.IsTouristTaxable)
                touristTax += lineAfterDiscount * 0.04m;
            discounts += discount;

            return new InvoiceItem
            {
                InvoiceId = id,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = lineTotal,
                IsExempt = item.IsExempt,
                ISVRate = item.ISVRate,
                IsTouristTaxable = item.IsTouristTaxable,
                DiscountPercentage = item.DiscountPercentage
            };
        }).ToList();

        invoice.CustomerName = request.CustomerName;
        invoice.RTNCliente = request.RTNCliente;
        invoice.CustomerAddress = request.CustomerAddress;
        invoice.SubTotal = Math.Round(subtotal, 2);
        invoice.ISVAmount = Math.Round(isvAmount, 2);
        invoice.TouristTaxAmount = Math.Round(touristTax, 2);
        invoice.DiscountsAmount = Math.Round(discounts, 2);
        invoice.TotalAmount = Math.Round(subtotal + isvAmount + touristTax, 2);

        foreach (var oldItem in invoice.InvoiceItems)
            oldItem.IsDeleted = true;
        foreach (var item in items)
            invoice.InvoiceItems.Add(item);

        await context.SaveChangesAsync(cancellationToken);
        await auditService.LogAsync(
            userId,
            "UpdateInvoice",
            nameof(Invoice),
            invoice.Id,
            new { invoice.CorrelativeNumber, invoice.SubTotal, invoice.TotalAmount },
            invoice.CorrelativeNumber);
        await transaction.CommitAsync(cancellationToken);
        return InvoiceMutationResult.Success(invoice);
    }

    public async Task<InvoiceMutationResult> CreateAsync(
        Guid userId,
        CreateInvoiceRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
        {
            return InvoiceMutationResult.BadRequest(
                "La cabecera Idempotency-Key es obligatoria y debe contener un UUID válido.",
                "Clave de idempotencia inválida");
        }

        var requestHash = IdempotencyService.ComputeRequestHash(request);
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var priorAttempt = await idempotencyService.LockAndFindAsync(
                userId,
                "invoice:create",
                normalizedKey,
                requestHash,
                cancellationToken);
            if (priorAttempt is not null)
            {
                var existingInvoice = await repository.GetByIdAsync(priorAttempt.ResourceId);
                if (existingInvoice is null)
                {
                    return InvoiceMutationResult.Conflict(
                        "La operación ya fue registrada, pero su factura no está disponible.",
                        "Resultado idempotente inconsistente");
                }

                await transaction.CommitAsync(cancellationToken);
                return InvoiceMutationResult.Success(existingInvoice, normalizedKey);
            }
        }
        catch (IdempotencyConflictException exception)
        {
            return InvoiceMutationResult.Conflict(exception.Message, "Conflicto de idempotencia");
        }

        var profileBlockReason = await fiscalProfileService.GetOperationBlockReasonAsync();
        if (profileBlockReason is not null)
            return InvoiceMutationResult.Conflict(profileBlockReason, "Perfil fiscal no habilitado");

        if (!string.Equals(request.DocumentType, InvoiceDocumentType.Factura.ToString(), StringComparison.Ordinal))
            return InvoiceMutationResult.BadRequest("Este endpoint solo emite facturas; use el flujo específico para notas fiscales");

        Folio? settlementFolio = null;
        Discount? appliedDiscount = null;
        var settlementItems = new List<FolioItem>();
        if (request.FolioId.HasValue)
        {
            var folioLock = $"folio-settlement:{request.FolioId.Value:N}";
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({folioLock}));",
                cancellationToken);

            settlementFolio = await context.Folios
                .Include(folio => folio.FolioItems)
                .Include(folio => folio.Reservation)
                .Include(folio => folio.Room)
                .SingleOrDefaultAsync(folio => folio.Id == request.FolioId.Value, cancellationToken);
            if (settlementFolio is null)
                return InvoiceMutationResult.NotFound("Folio no encontrado");
            if (settlementFolio.Status != FolioStatus.Abierto
                || settlementFolio.Reservation.Status != ReservationStatus.CheckIn)
                return InvoiceMutationResult.Conflict("El folio no se encuentra abierto para liquidación");
            if (!request.GuestId.HasValue || request.GuestId.Value != settlementFolio.GuestId)
                return InvoiceMutationResult.BadRequest("El huésped de la factura no coincide con el folio");
            if (await context.Invoices.AnyAsync(invoice => invoice.FolioId == settlementFolio.Id, cancellationToken))
                return InvoiceMutationResult.Conflict("El folio ya tiene una factura de liquidación");

            settlementItems = settlementFolio.FolioItems
                .OrderBy(item => item.CreatedAt)
                .ThenBy(item => item.Id)
                .ToList();
            if (settlementItems.Count == 0)
                return InvoiceMutationResult.BadRequest("El folio no contiene cargos para facturar");

            if (request.DiscountId.HasValue)
            {
                appliedDiscount = await context.Discounts
                    .SingleOrDefaultAsync(discount => discount.Id == request.DiscountId.Value, cancellationToken);
                if (appliedDiscount is null || !appliedDiscount.IsActive)
                    return InvoiceMutationResult.BadRequest("El descuento seleccionado no existe o no está activo");
                if (appliedDiscount.DiscountType != DiscountType.Porcentaje)
                    return InvoiceMutationResult.BadRequest("La liquidación del folio solo admite descuentos porcentuales");
                if (appliedDiscount.Value is < 0m or > 100m)
                    return InvoiceMutationResult.BadRequest("El porcentaje del descuento configurado no es válido");
            }
        }
        else if (request.DiscountId.HasValue
            || request.CashRegisterId.HasValue
            || request.CashReceived.HasValue
            || !string.IsNullOrWhiteSpace(request.PaymentReference))
        {
            return InvoiceMutationResult.BadRequest("Caja y descuento de liquidación solo pueden indicarse al facturar un folio");
        }

        var cai = await caiRepository.GetByIdAsync(request.CAIId);
        if (cai is null)
            return InvoiceMutationResult.BadRequest("CAI no encontrado");
        if (cai.Status != CAIStatus.Activo)
            return InvoiceMutationResult.BadRequest("El CAI no está activo");
        if (cai.DueDate < clock.Today)
            return InvoiceMutationResult.BadRequest("El CAI está vencido");

        FiscalCorrelativeResult? authorizationResult = null;
        string correlative;
        try
        {
            if (request.DocumentAuthorizationId.HasValue)
            {
                authorizationResult = await fiscalAuthorizationService.GetNextCorrelativeAsync(
                    InvoiceDocumentType.Factura,
                    request.DocumentAuthorizationId);
                correlative = authorizationResult.CorrelativeNumber;
            }
            else
            {
                correlative = await caiCorrelativeService.GetNextAsync(request.CAIId);
            }
        }
        catch (InvalidOperationException exception)
        {
            return InvoiceMutationResult.BadRequest(exception.Message);
        }

        var taxpayerType = Enum.TryParse<TaxpayerType>(request.TaxpayerType, out var parsedType)
            ? parsedType
            : TaxpayerType.ConsumidorFinal;
        var isIsvExempt = taxpayerType == TaxpayerType.Exonerado && request.IsIsvExempt;
        var isTouristTaxExempt = taxpayerType == TaxpayerType.Exonerado && request.IsTouristTaxExempt;
        if (taxpayerType == TaxpayerType.Exonerado
            && (string.IsNullOrWhiteSpace(request.ExonerationOrderNumber)
                || string.IsNullOrWhiteSpace(request.SefinExonerationCertificateNumber)))
        {
            return InvoiceMutationResult.BadRequest("Cliente exonerado requiere O.C. Exenta y Constancia SEFIN");
        }

        InvoiceCalculation calculation;
        try
        {
            var calculationLines = settlementFolio is null
                ? request.Items.Select(ToCalculationLine)
                : settlementItems.Select(item => new InvoiceLineInput(
                    item.Description,
                    item.Quantity,
                    item.UnitPrice,
                    item.IsExempt,
                    item.ISVRate,
                    item.IsTouristTaxable,
                    CombineDiscountPercent(item.DiscountPercentage, appliedDiscount?.Value ?? 0m)));
            calculation = invoiceCalculator.Calculate(
                calculationLines,
                isIsvExempt,
                isTouristTaxExempt);

            for (var index = 0; index < settlementItems.Count; index++)
                calculation.Items[index].FolioItemId = settlementItems[index].Id;
        }
        catch (InvoiceCalculationException exception)
        {
            return InvoiceMutationResult.BadRequest(exception.Message);
        }

        decimal? cashBalanceAfter = null;
        if (settlementFolio is not null && request.PaymentMethod is "Tarjeta" or "Transferencia")
        {
            if (request.CashRegisterId.HasValue || request.CashReceived.HasValue)
                return InvoiceMutationResult.BadRequest("Tarjeta y transferencia no deben afectar la caja de efectivo");
            if ((request.PaymentReference?.Trim().Length ?? 0) < 3)
                return InvoiceMutationResult.BadRequest("Tarjeta y transferencia requieren una referencia de al menos 3 caracteres");
        }
        if (settlementFolio is not null && request.PaymentMethod == "Efectivo")
        {
            if (!request.CashRegisterId.HasValue)
                return InvoiceMutationResult.BadRequest("Debe seleccionar una caja abierta para registrar el pago en efectivo");
            if (!request.CashReceived.HasValue || request.CashReceived.Value < calculation.TotalAmount)
                return InvoiceMutationResult.BadRequest("El efectivo recibido debe cubrir el total de la factura");

            var cashLock = $"cash-register:{request.CashRegisterId.Value:N}";
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({cashLock}));",
                cancellationToken);
            var cashRegister = await context.CashRegisters
                .SingleOrDefaultAsync(
                    register => register.Id == request.CashRegisterId.Value && register.IsActive,
                    cancellationToken);
            if (cashRegister is null)
                return InvoiceMutationResult.BadRequest("La caja seleccionada no existe o está inactiva");

            var lastMovement = await context.CashMovements
                .Where(movement => movement.CashRegisterId == cashRegister.Id)
                .OrderByDescending(movement => movement.CreatedAt)
                .ThenByDescending(movement => movement.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (lastMovement is null || lastMovement.MovementType == CashMovementType.Cierre)
                return InvoiceMutationResult.Conflict("La caja seleccionada está cerrada");
            cashBalanceAfter = TaxService.RoundCurrency(lastMovement.BalanceAfter + calculation.TotalAmount);
        }

        var invoice = new Invoice
        {
            CAIId = request.CAIId,
            DocumentAuthorizationId = authorizationResult?.AuthorizationId,
            CAINumberSnapshot = authorizationResult?.CAINumber ?? cai.CAINumber,
            AuthorizationRangeSnapshot = authorizationResult is not null
                ? $"{authorizationResult.InitialRange} - {authorizationResult.FinalRange}"
                : $"{cai.InitialRange} - {cai.FinalRange}",
            AuthorizationDueDateSnapshot = authorizationResult?.DueDate.ToDateTime(TimeOnly.MinValue)
                ?? DateTime.SpecifyKind(cai.DueDate.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc),
            CorrelativeNumber = correlative,
            CustomerId = request.CustomerId,
            GuestId = request.GuestId,
            FolioId = settlementFolio?.Id,
            RTNCliente = request.RTNCliente,
            CustomerName = request.CustomerName,
            CustomerAddress = request.CustomerAddress,
            PaymentMethod = settlementFolio is not null ? request.PaymentMethod : null,
            SubTotal = calculation.SubTotal,
            ISVAmount = calculation.ISVAmount,
            ISV15Amount = calculation.ISV15Amount,
            ISV18Amount = calculation.ISV18Amount,
            TouristTaxAmount = calculation.TouristTaxAmount,
            DiscountsAmount = calculation.DiscountsAmount,
            AppliedDiscountId = appliedDiscount?.Id,
            AppliedDiscountNameSnapshot = appliedDiscount?.Name,
            AppliedDiscountPercentageSnapshot = appliedDiscount?.Value,
            TotalAmount = calculation.TotalAmount,
            TaxableAmount = calculation.TaxableAmount,
            ExoneratedAmount = calculation.ExoneratedAmount,
            ExemptAmount = calculation.ExemptAmount,
            TaxpayerType = taxpayerType,
            ExonerationOrderNumber = request.ExonerationOrderNumber,
            SefinExonerationCertificateNumber = request.SefinExonerationCertificateNumber,
            SagRegistryNumber = request.SagRegistryNumber,
            IsIsvExempt = isIsvExempt,
            IsTouristTaxExempt = isTouristTaxExempt,
            OriginalInvoiceId = request.OriginalInvoiceId,
            OriginalCorrelativeNumber = request.OriginalInvoiceId.HasValue
                ? (await repository.GetByIdAsync(request.OriginalInvoiceId.Value))?.CorrelativeNumber
                : null,
            Reason = request.Reason,
            DocumentType = InvoiceDocumentType.Factura,
            Status = settlementFolio is not null ? InvoiceStatus.Pagada : InvoiceStatus.Emitida,
            InvoiceDate = clock.HondurasNow,
            CashReceived = settlementFolio is not null && request.PaymentMethod == "Efectivo"
                ? request.CashReceived
                : null,
            CashChange = settlementFolio is not null && request.PaymentMethod == "Efectivo"
                ? TaxService.RoundCurrency(request.CashReceived!.Value - calculation.TotalAmount)
                : null,
            InvoiceItems = calculation.Items.ToList()
        };

        Payment? settlementPayment = null;
        if (settlementFolio is not null)
        {
            var paymentId = Guid.CreateVersion7();
            settlementPayment = new Payment
            {
                Id = paymentId,
                PaymentNumber = $"PAG-{paymentId:N}".ToUpperInvariant(),
                RecordedByUserId = userId,
                Method = Enum.Parse<PaymentMethod>(request.PaymentMethod),
                Currency = "HNL",
                Amount = calculation.TotalAmount,
                CashReceived = request.PaymentMethod == "Efectivo" ? request.CashReceived : null,
                CashChange = request.PaymentMethod == "Efectivo"
                    ? TaxService.RoundCurrency(request.CashReceived!.Value - calculation.TotalAmount)
                    : null,
                ExternalReference = request.PaymentMethod == "Efectivo"
                    ? string.Empty
                    : request.PaymentReference!.Trim(),
                PaymentDate = clock.HondurasNow,
                Status = PaymentStatus.Confirmado,
                CashRegisterId = request.PaymentMethod == "Efectivo" ? request.CashRegisterId : null
            };
            var paymentApplication = new PaymentApplication
            {
                Id = Guid.CreateVersion7(),
                Payment = settlementPayment,
                Invoice = invoice,
                Amount = calculation.TotalAmount
            };
            settlementPayment.Applications.Add(paymentApplication);
            invoice.PaymentApplications.Add(paymentApplication);
        }

        if (settlementFolio is not null)
        {
            settlementFolio.TotalAmount = calculation.TotalAmount;
            settlementFolio.ClosingDate = clock.HondurasNow;
            settlementFolio.Status = FolioStatus.Cerrado;
            settlementFolio.Reservation.Status = ReservationStatus.CheckOut;
            settlementFolio.Reservation.Version++;
            settlementFolio.Room.Status = RoomStatus.Limpieza;
        }

        ApplyFiscalSnapshot(invoice);
        await context.Invoices.AddAsync(invoice, cancellationToken);
        if (settlementFolio is not null && request.PaymentMethod == "Efectivo")
        {
            context.CashMovements.Add(new CashMovement
            {
                CashRegisterId = request.CashRegisterId!.Value,
                UserId = userId,
                MovementType = CashMovementType.Ingreso,
                Amount = calculation.TotalAmount,
                Description = $"Cobro {settlementPayment!.PaymentNumber} - Factura {invoice.CorrelativeNumber}",
                MovementDate = clock.HondurasNow,
                BalanceAfter = cashBalanceAfter!.Value,
                ReferenceId = settlementPayment.Id
            });
        }

        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.InvoiceAfterPersistence, cancellationToken);
        await accountingService.CreateInvoiceEntryAsync(invoice);
        if (settlementPayment is not null)
            await accountingService.CreatePaymentEntryAsync(settlementPayment);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.InvoiceAfterAccounting, cancellationToken);

        if (settlementPayment is not null)
        {
            await auditService.LogAsync(
                userId,
                "CreatePayment",
                nameof(Payment),
                settlementPayment.Id,
                new
                {
                    settlementPayment.PaymentNumber,
                    settlementPayment.Method,
                    settlementPayment.Amount,
                    InvoiceId = invoice.Id,
                    invoice.CorrelativeNumber,
                    settlementPayment.CashRegisterId,
                    settlementPayment.ExternalReference
                },
                invoice.CorrelativeNumber,
                settlementPayment.Method.ToString());
        }
        await auditService.LogAsync(
            userId,
            "CreateInvoice",
            nameof(Invoice),
            invoice.Id,
            new { invoice.CorrelativeNumber, invoice.TotalAmount, invoice.TaxpayerType },
            invoice.CorrelativeNumber,
            invoice.PaymentMethod);
        if (settlementFolio is not null)
        {
            await auditService.LogAsync(
                userId,
                "CheckOut",
                nameof(Reservation),
                settlementFolio.ReservationId,
                new
                {
                    settlementFolio.Id,
                    InvoiceId = invoice.Id,
                    invoice.CorrelativeNumber,
                    settlementFolio.RoomId,
                    settlementFolio.TotalAmount
                },
                invoice.CorrelativeNumber,
                invoice.PaymentMethod);
        }
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.InvoiceAfterAudit, cancellationToken);
        await idempotencyService.StoreAsync(
            userId,
            "invoice:create",
            normalizedKey,
            requestHash,
            invoice.Id,
            cancellationToken);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.InvoiceAfterIdempotency, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return InvoiceMutationResult.Success(invoice, normalizedKey);
    }

    public async Task<InvoiceMutationResult> CreateCreditNoteAsync(
        Guid originalInvoiceId,
        Guid userId,
        CreateCreditNoteRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
        {
            return InvoiceMutationResult.BadRequest(
                "La cabecera Idempotency-Key es obligatoria y debe contener un UUID válido.",
                "Clave de idempotencia inválida");
        }
        if (request.OriginalInvoiceId != originalInvoiceId)
            return InvoiceMutationResult.BadRequest("La factura original de la ruta no coincide con la solicitud");

        var requestHash = IdempotencyService.ComputeRequestHash(request);
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var priorAttempt = await idempotencyService.LockAndFindAsync(
                userId,
                "invoice:credit-note",
                normalizedKey,
                requestHash,
                cancellationToken);
            if (priorAttempt is not null)
            {
                var existing = await repository.GetByIdAsync(priorAttempt.ResourceId);
                if (existing is null)
                {
                    return InvoiceMutationResult.Conflict(
                        "La operación ya fue registrada, pero su documento no está disponible.",
                        "Resultado idempotente inconsistente");
                }

                await transaction.CommitAsync(cancellationToken);
                return InvoiceMutationResult.Success(existing, normalizedKey);
            }
        }
        catch (IdempotencyConflictException exception)
        {
            return InvoiceMutationResult.Conflict(exception.Message, "Conflicto de idempotencia");
        }

        var profileBlockReason = await fiscalProfileService.GetOperationBlockReasonAsync();
        if (profileBlockReason is not null)
            return InvoiceMutationResult.Conflict(profileBlockReason, "Perfil fiscal no habilitado");

        var adjustmentLock = $"invoice-balance:{originalInvoiceId:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({adjustmentLock}));",
            cancellationToken);

        var original = await context.Invoices
            .Include(invoice => invoice.InvoiceItems)
            .SingleOrDefaultAsync(invoice => invoice.Id == originalInvoiceId, cancellationToken);
        if (original is null)
            return InvoiceMutationResult.NotFound("Factura original no encontrada");
        if (original.DocumentType != InvoiceDocumentType.Factura)
            return InvoiceMutationResult.BadRequest("La nota de crédito debe vincularse a una factura");
        if (original.Status == InvoiceStatus.Anulada)
            return InvoiceMutationResult.Conflict("La factura ya fue acreditada por completo");

        var requestedGroups = request.Items.GroupBy(item => item.OriginalInvoiceItemId).ToList();
        if (requestedGroups.Any(group => group.Count() > 1))
            return InvoiceMutationResult.BadRequest("Cada línea original debe aparecer una sola vez en la nota de crédito");

        var originalLines = original.InvoiceItems.ToDictionary(item => item.Id);
        if (request.Items.Any(item => !originalLines.ContainsKey(item.OriginalInvoiceItemId)))
            return InvoiceMutationResult.BadRequest("La nota de crédito contiene una línea que no pertenece a la factura original");

        var creditedQuantities = await context.InvoiceItems
            .Where(item => item.OriginalInvoiceItemId.HasValue
                && item.Invoice.OriginalInvoiceId == originalInvoiceId
                && item.Invoice.DocumentType == InvoiceDocumentType.NotaCredito)
            .GroupBy(item => item.OriginalInvoiceItemId!.Value)
            .Select(group => new { OriginalItemId = group.Key, Quantity = group.Sum(item => item.Quantity) })
            .ToDictionaryAsync(item => item.OriginalItemId, item => item.Quantity, cancellationToken);

        foreach (var requested in request.Items)
        {
            var originalLine = originalLines[requested.OriginalInvoiceItemId];
            var remaining = originalLine.Quantity - creditedQuantities.GetValueOrDefault(originalLine.Id);
            if (requested.Quantity > remaining)
            {
                return InvoiceMutationResult.Conflict(
                    $"La cantidad acreditada para '{originalLine.Description}' excede el saldo disponible");
            }
        }

        InvoiceCalculation calculation;
        try
        {
            calculation = invoiceCalculator.Calculate(
                request.Items.Select(requested =>
                {
                    var source = originalLines[requested.OriginalInvoiceItemId];
                    return new InvoiceLineInput(
                        source.Description,
                        requested.Quantity,
                        source.UnitPrice,
                        source.IsExempt,
                        source.ISVRate,
                        source.IsTouristTaxable,
                        source.DiscountPercentage,
                        source.Id);
                }),
                original.IsIsvExempt,
                original.IsTouristTaxExempt);
        }
        catch (InvoiceCalculationException exception)
        {
            return InvoiceMutationResult.BadRequest(exception.Message);
        }

        var caiFailure = await ValidateActiveCaiAsync(request.CAIId);
        if (caiFailure is not null)
            return caiFailure;

        FiscalCorrelativeResult authorization;
        try
        {
            authorization = await fiscalAuthorizationService.GetNextCorrelativeAsync(
                InvoiceDocumentType.NotaCredito,
                request.DocumentAuthorizationId);
        }
        catch (InvalidOperationException exception)
        {
            return InvoiceMutationResult.BadRequest(exception.Message);
        }

        var creditNote = CreateAdjustmentInvoice(
            original,
            request.CAIId,
            request.GuestId,
            request.Reason,
            InvoiceDocumentType.NotaCredito,
            authorization,
            calculation);

        var requestedByLine = request.Items.ToDictionary(item => item.OriginalInvoiceItemId, item => item.Quantity);
        var fullyCredited = original.InvoiceItems.All(line =>
            creditedQuantities.GetValueOrDefault(line.Id) + requestedByLine.GetValueOrDefault(line.Id) == line.Quantity);
        if (fullyCredited)
            original.Status = InvoiceStatus.Anulada;

        ApplyFiscalSnapshot(creditNote);
        await context.Invoices.AddAsync(creditNote, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.CreditNoteAfterPersistence, cancellationToken);
        await accountingService.CreateInvoiceEntryAsync(creditNote);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.CreditNoteAfterAccounting, cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateCreditNote",
            nameof(Invoice),
            creditNote.Id,
            new
            {
                creditNote.CorrelativeNumber,
                Original = original.CorrelativeNumber,
                creditNote.TotalAmount,
                Reason = creditNote.Reason
            },
            creditNote.CorrelativeNumber);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.CreditNoteAfterAudit, cancellationToken);
        await idempotencyService.StoreAsync(
            userId,
            "invoice:credit-note",
            normalizedKey,
            requestHash,
            creditNote.Id,
            cancellationToken);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.CreditNoteAfterIdempotency, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return InvoiceMutationResult.Success(creditNote, normalizedKey);
    }

    public async Task<InvoiceMutationResult> CreateDebitNoteAsync(
        Guid originalInvoiceId,
        Guid userId,
        CreateDebitNoteRequest request,
        string? idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (!IdempotencyService.TryNormalizeKey(idempotencyKey, out var normalizedKey))
        {
            return InvoiceMutationResult.BadRequest(
                "La cabecera Idempotency-Key es obligatoria y debe contener un UUID válido.",
                "Clave de idempotencia inválida");
        }
        if (request.OriginalInvoiceId != originalInvoiceId)
            return InvoiceMutationResult.BadRequest("La factura original de la ruta no coincide con la solicitud");

        var requestHash = IdempotencyService.ComputeRequestHash(request);
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var priorAttempt = await idempotencyService.LockAndFindAsync(
                userId,
                "invoice:debit-note",
                normalizedKey,
                requestHash,
                cancellationToken);
            if (priorAttempt is not null)
            {
                var existing = await repository.GetByIdAsync(priorAttempt.ResourceId);
                if (existing is null)
                {
                    return InvoiceMutationResult.Conflict(
                        "La operación ya fue registrada, pero su documento no está disponible.",
                        "Resultado idempotente inconsistente");
                }

                await transaction.CommitAsync(cancellationToken);
                return InvoiceMutationResult.Success(existing, normalizedKey);
            }
        }
        catch (IdempotencyConflictException exception)
        {
            return InvoiceMutationResult.Conflict(exception.Message, "Conflicto de idempotencia");
        }

        var profileBlockReason = await fiscalProfileService.GetOperationBlockReasonAsync();
        if (profileBlockReason is not null)
            return InvoiceMutationResult.Conflict(profileBlockReason, "Perfil fiscal no habilitado");

        var adjustmentLock = $"invoice-adjust:{originalInvoiceId:N}";
        await context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtext({adjustmentLock}));",
            cancellationToken);

        var original = await context.Invoices
            .SingleOrDefaultAsync(invoice => invoice.Id == originalInvoiceId, cancellationToken);
        if (original is null)
            return InvoiceMutationResult.NotFound("Factura original no encontrada");
        if (original.DocumentType != InvoiceDocumentType.Factura)
            return InvoiceMutationResult.BadRequest("La nota de débito debe vincularse a una factura");
        if (original.Status == InvoiceStatus.Anulada)
            return InvoiceMutationResult.Conflict("No se puede cargar una factura acreditada por completo");

        InvoiceCalculation calculation;
        try
        {
            calculation = invoiceCalculator.Calculate(
                request.Items.Select(ToCalculationLine),
                original.IsIsvExempt,
                original.IsTouristTaxExempt);
        }
        catch (InvoiceCalculationException exception)
        {
            return InvoiceMutationResult.BadRequest(exception.Message);
        }

        var caiFailure = await ValidateActiveCaiAsync(request.CAIId);
        if (caiFailure is not null)
            return caiFailure;

        FiscalCorrelativeResult authorization;
        try
        {
            authorization = await fiscalAuthorizationService.GetNextCorrelativeAsync(
                InvoiceDocumentType.NotaDebito,
                request.DocumentAuthorizationId);
        }
        catch (InvalidOperationException exception)
        {
            return InvoiceMutationResult.BadRequest(exception.Message);
        }

        var debitNote = CreateAdjustmentInvoice(
            original,
            request.CAIId,
            request.GuestId,
            request.Reason,
            InvoiceDocumentType.NotaDebito,
            authorization,
            calculation);

        ApplyFiscalSnapshot(debitNote);
        await context.Invoices.AddAsync(debitNote, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.DebitNoteAfterPersistence, cancellationToken);
        await accountingService.CreateInvoiceEntryAsync(debitNote);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.DebitNoteAfterAccounting, cancellationToken);
        await auditService.LogAsync(
            userId,
            "CreateDebitNote",
            nameof(Invoice),
            debitNote.Id,
            new
            {
                debitNote.CorrelativeNumber,
                Original = original.CorrelativeNumber,
                debitNote.TotalAmount,
                Reason = debitNote.Reason
            },
            debitNote.CorrelativeNumber);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.DebitNoteAfterAudit, cancellationToken);
        await idempotencyService.StoreAsync(
            userId,
            "invoice:debit-note",
            normalizedKey,
            requestHash,
            debitNote.Id,
            cancellationToken);
        await failureInjector.CheckpointAsync(ApplicationFailureCheckpoints.DebitNoteAfterIdempotency, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return InvoiceMutationResult.Success(debitNote, normalizedKey);
    }

    private async Task<InvoiceMutationResult?> ValidateActiveCaiAsync(Guid caiId)
    {
        var cai = await caiRepository.GetByIdAsync(caiId);
        if (cai is null)
            return InvoiceMutationResult.BadRequest("CAI no encontrado");
        if (cai.Status != CAIStatus.Activo)
            return InvoiceMutationResult.BadRequest("El CAI no está activo");
        if (cai.DueDate < clock.Today)
            return InvoiceMutationResult.BadRequest("El CAI está vencido");

        return null;
    }

    private Invoice CreateAdjustmentInvoice(
        Invoice original,
        Guid caiId,
        Guid? guestId,
        string reason,
        InvoiceDocumentType documentType,
        FiscalCorrelativeResult authorization,
        InvoiceCalculation calculation)
        => new()
        {
            CAIId = caiId,
            DocumentAuthorizationId = authorization.AuthorizationId,
            CAINumberSnapshot = authorization.CAINumber,
            AuthorizationRangeSnapshot = $"{authorization.InitialRange} - {authorization.FinalRange}",
            AuthorizationDueDateSnapshot = DateTime.SpecifyKind(
                authorization.DueDate.ToDateTime(TimeOnly.MaxValue),
                DateTimeKind.Utc),
            CorrelativeNumber = authorization.CorrelativeNumber,
            OriginalInvoiceId = original.Id,
            OriginalCorrelativeNumber = original.CorrelativeNumber,
            Reason = reason.Trim(),
            CustomerId = original.CustomerId,
            GuestId = original.GuestId ?? guestId,
            RTNCliente = original.RTNCliente,
            CustomerName = original.CustomerName,
            CustomerAddress = original.CustomerAddress,
            SubTotal = calculation.SubTotal,
            ISVAmount = calculation.ISVAmount,
            ISV15Amount = calculation.ISV15Amount,
            ISV18Amount = calculation.ISV18Amount,
            TouristTaxAmount = calculation.TouristTaxAmount,
            DiscountsAmount = calculation.DiscountsAmount,
            TotalAmount = calculation.TotalAmount,
            TaxableAmount = calculation.TaxableAmount,
            ExemptAmount = calculation.ExemptAmount,
            ExoneratedAmount = calculation.ExoneratedAmount,
            TaxpayerType = original.TaxpayerType,
            ExonerationOrderNumber = original.ExonerationOrderNumber,
            SefinExonerationCertificateNumber = original.SefinExonerationCertificateNumber,
            SagRegistryNumber = original.SagRegistryNumber,
            IsIsvExempt = original.IsIsvExempt,
            IsTouristTaxExempt = original.IsTouristTaxExempt,
            DocumentType = documentType,
            Status = InvoiceStatus.Emitida,
            InvoiceDate = clock.HondurasNow,
            InvoiceItems = calculation.Items.ToList()
        };

    private static InvoiceLineInput ToCalculationLine(InvoiceItemDto item)
        => new(
            item.Description,
            item.Quantity,
            item.UnitPrice,
            item.IsExempt,
            item.ISVRate,
            item.IsTouristTaxable,
            item.DiscountPercentage,
            item.OriginalInvoiceItemId);

    private static void ApplyFiscalSnapshot(Invoice invoice)
    {
        var snapshot = new
        {
            invoice.CAIId,
            invoice.DocumentAuthorizationId,
            invoice.CAINumberSnapshot,
            invoice.AuthorizationRangeSnapshot,
            invoice.AuthorizationDueDateSnapshot,
            invoice.CorrelativeNumber,
            invoice.InvoiceDate,
            invoice.CustomerName,
            invoice.RTNCliente,
            invoice.CustomerAddress,
            invoice.FolioId,
            invoice.DocumentType,
            invoice.Status,
            invoice.TaxpayerType,
            invoice.SubTotal,
            invoice.ISVAmount,
            invoice.TouristTaxAmount,
            invoice.DiscountsAmount,
            invoice.AppliedDiscountId,
            invoice.AppliedDiscountNameSnapshot,
            invoice.AppliedDiscountPercentageSnapshot,
            invoice.TotalAmount,
            invoice.TaxableAmount,
            invoice.ExemptAmount,
            invoice.ExoneratedAmount,
            invoice.IsIsvExempt,
            invoice.IsTouristTaxExempt,
            invoice.ExonerationOrderNumber,
            invoice.SefinExonerationCertificateNumber,
            invoice.SagRegistryNumber,
            invoice.OriginalInvoiceId,
            invoice.OriginalCorrelativeNumber,
            invoice.Reason,
            invoice.PaymentMethod,
            invoice.CashReceived,
            invoice.CashChange,
            Items = invoice.InvoiceItems.Select(item => new
            {
                item.OriginalInvoiceItemId,
                item.FolioItemId,
                item.Description,
                item.Quantity,
                item.UnitPrice,
                item.LineTotal,
                item.IsExempt,
                item.ISVRate,
                item.IsTouristTaxable,
                item.DiscountPercentage
            }).ToList()
        };

        invoice.FiscalSnapshotJson = JsonSerializer.Serialize(
            snapshot,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        invoice.FiscalHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(invoice.FiscalSnapshotJson)));
    }

    private static decimal CombineDiscountPercent(decimal lineDiscount, decimal settlementDiscount)
    {
        var remainingPercent = (100m - lineDiscount) * (100m - settlementDiscount) / 100m;
        return Math.Round(100m - remainingPercent, 6, MidpointRounding.AwayFromZero);
    }
}
