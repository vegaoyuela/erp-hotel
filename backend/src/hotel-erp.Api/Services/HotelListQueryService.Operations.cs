using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Common;
using hotel_erp.Api.Dtos.Customer;
using hotel_erp.Api.Dtos.Inventory;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed partial class HotelListQueryService
{
    public async Task<PagedResult<ReservationDto>> ReservationsAsync(ReservationPageQuery request, CancellationToken token)
    {
        var query = context.Reservations.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = Enum.Parse<ReservationStatus>(request.Status);
            query = query.Where(reservation => reservation.Status == status);
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(reservation => (reservation.Guest.FirstName + " " + reservation.Guest.LastName).ToLower().Contains(term)
                || reservation.Room.RoomNumber.ToLower().Contains(term)
                || (reservation.Guest.DocumentNumber != null && reservation.Guest.DocumentNumber.ToLower().Contains(term)));
        }
        return await query.Select(reservation => new ReservationDto
        {
            Id = reservation.Id,
            GuestId = reservation.GuestId,
            GuestName = reservation.Guest.FirstName + " " + reservation.Guest.LastName,
            RoomId = reservation.RoomId,
            RoomNumber = reservation.Room.RoomNumber,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            Adults = reservation.Adults,
            Children = reservation.Children,
            PaymentMethod = reservation.PaymentMethod,
            AdvancePayment = reservation.AdvancePayment,
            Status = reservation.Status.ToString(),
            Notes = reservation.Notes,
            Version = reservation.Version
        }).OrderByDescending(reservation => reservation.CheckInDate).ThenByDescending(reservation => reservation.Id)
            .ToPageAsync(request, token);
    }

    public async Task<PagedResult<FolioListDto>> FoliosAsync(FolioPageQuery request, CancellationToken token)
    {
        // Keep historical guest/room names even after a related catalog row is soft-deleted.
        var query = context.Folios.IgnoreQueryFilters().AsNoTracking().Where(folio => !folio.IsDeleted);
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = Enum.Parse<FolioStatus>(request.Status);
            query = query.Where(folio => folio.Status == status);
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(folio => (folio.Guest.FirstName + " " + folio.Guest.LastName).ToLower().Contains(term)
                || folio.Room.RoomNumber.ToLower().Contains(term));
        }
        return await query.Select(folio => new FolioListDto
        {
            Id = folio.Id,
            GuestName = folio.Guest.FirstName + " " + folio.Guest.LastName,
            RoomNumber = folio.Room.RoomNumber,
            OpeningDate = folio.OpeningDate,
            ClosingDate = folio.ClosingDate,
            TotalAmount = folio.TotalAmount,
            Status = folio.Status.ToString(),
            ItemCount = folio.FolioItems.Count(item => !item.IsDeleted)
        })
            .OrderByDescending(folio => folio.OpeningDate).ThenByDescending(folio => folio.Id).ToPageAsync(request, token);
    }

    public async Task<PagedResult<CustomerDto>> CustomersAsync(SearchPageQuery request, CancellationToken token)
    {
        var query = context.Customers.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(customer => customer.Name.ToLower().Contains(term)
                || (customer.RTN != null && customer.RTN.ToLower().Contains(term))
                || (customer.Phone != null && customer.Phone.ToLower().Contains(term))
                || (customer.Email != null && customer.Email.ToLower().Contains(term)));
        }
        var page = await query.OrderBy(customer => customer.Name).ThenBy(customer => customer.Id).ToPageAsync(request, token);
        return new(mapper.Map<List<CustomerDto>>(page.Items), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public async Task<PagedResult<ProductDto>> ProductsAsync(ProductPageQuery request, CancellationToken token)
    {
        var query = context.Products.AsNoTracking();
        if (request.CategoryId.HasValue) query = query.Where(product => product.CategoryId == request.CategoryId);
        if (request.IsActive.HasValue) query = query.Where(product => product.IsActive == request.IsActive);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(product => product.Name.ToLower().Contains(term)
                || (product.SKU != null && product.SKU.ToLower().Contains(term))
                || (product.Description != null && product.Description.ToLower().Contains(term))
                || (product.Category != null && product.Category.Name.ToLower().Contains(term)));
        }
        return await query.Select(product => new ProductDto
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
        }).OrderBy(product => product.Name).ThenBy(product => product.Id).ToPageAsync(request, token);
    }

    public async Task<PagedResult<InventoryMovementDto>> InventoryMovementsAsync(InventoryMovementPageQuery request, CancellationToken token)
    {
        var query = context.InventoryMovements.AsNoTracking();
        if (request.ProductId.HasValue) query = query.Where(movement => movement.ProductId == request.ProductId);
        if (!string.IsNullOrWhiteSpace(request.MovementType))
        {
            var type = Enum.Parse<InventoryMovementType>(request.MovementType);
            query = query.Where(movement => movement.MovementType == type);
        }
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            query = query.Where(movement => movement.Product.Name.ToLower().Contains(term)
                || (movement.Product.SKU != null && movement.Product.SKU.ToLower().Contains(term)));
        }
        return await query.Select(movement => new InventoryMovementDto
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
        }).OrderByDescending(movement => movement.MovementDate).ThenByDescending(movement => movement.Id)
            .ToPageAsync(request, token);
    }

    public async Task<InventorySummaryDto> InventorySummaryAsync(CancellationToken token)
    {
        var counts = await context.Products.AsNoTracking().GroupBy(_ => 1).Select(group => new
        {
            Total = group.Count(),
            Active = group.Count(product => product.IsActive),
            Low = group.Count(product => product.IsActive && product.CurrentStock <= product.MinStockLevel)
        }).SingleOrDefaultAsync(token);
        return new(counts?.Total ?? 0, counts?.Active ?? 0, counts?.Low ?? 0,
            await context.InventoryMovements.CountAsync(token));
    }
}
