using hotel_erp.Api.Authorization;
using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Dashboard;
using hotel_erp.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController(ApplicationDbContext context, IHotelClock clock) : ControllerBase
{
    // Every authenticated operator has a home page; each section requires its own permission.
    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        bool Has(string permission) => User.HasClaim(SecurityClaimTypes.Permission, permission);
        bool Can(string policy) => HotelAccessPolicies.AnyOf[policy].Any(Has);
        var token = HttpContext.RequestAborted;
        var today = clock.Today;
        var summary = new DashboardSummaryDto();
        if (Has(PermissionNames.ManageReservations))
        {
            summary.OccupiedRooms = await context.Rooms.CountAsync(row => row.Status == RoomStatus.Ocupada, token);
            summary.FreeRooms = await context.Rooms.CountAsync(row => row.Status == RoomStatus.Libre, token);
            summary.PendingCheckIns = await context.Reservations.CountAsync(row => row.CheckInDate == today && row.Status == ReservationStatus.Confirmada, token);
            summary.PendingCheckOuts = await context.Reservations.CountAsync(row => row.CheckOutDate == today && row.Status == ReservationStatus.CheckIn, token);
            summary.UpcomingReservations = await context.Reservations.AsNoTracking()
                .Where(row => row.Status == ReservationStatus.Confirmada || row.Status == ReservationStatus.Pendiente)
                .OrderBy(row => row.CheckInDate).ThenBy(row => row.Id).Take(5)
                .Select(row => new DashboardReservationDto
                {
                    GuestName = row.Guest.FirstName + " " + row.Guest.LastName,
                    RoomNumber = row.Room.RoomNumber,
                    Status = row.Status.ToString(),
                    CheckInDate = row.CheckInDate,
                    CheckOutDate = row.CheckOutDate
                }).ToListAsync(token);
            summary.Stats.AddRange([
                new() { Title = "Habitaciones ocupadas", Value = summary.OccupiedRooms.ToString(), Trend = "Operativo" },
                new() { Title = "Habitaciones libres", Value = summary.FreeRooms.ToString(), Trend = "Operativo" },
                new() { Title = "Reservas hoy", Value = summary.PendingCheckIns.ToString(), Trend = "Por confirmar" }
            ]);
        }
        if (Can(HotelAccessPolicies.ReadInvoices))
        {
            summary.RecentInvoices = await context.Invoices.AsNoTracking().OrderByDescending(row => row.InvoiceDate)
                .ThenBy(row => row.Id).Take(5).Select(row => new DashboardInvoiceDto
                {
                    CorrelativeNumber = row.CorrelativeNumber,
                    CustomerName = row.CustomerName,
                    TotalAmount = row.TotalAmount,
                    Status = row.Status.ToString(),
                    InvoiceDate = row.InvoiceDate
                }).ToListAsync(token);
            var start = clock.HondurasNow.Date;
            var end = start.AddDays(1);
            var invoices = context.Invoices.Where(row => row.InvoiceDate >= start && row.InvoiceDate < end);
            var count = await invoices.CountAsync(token);
            var amount = await invoices.SumAsync(row => (decimal?)row.TotalAmount, token) ?? 0m;
            summary.Stats.Add(new() { Title = "Ingresos del día", Value = $"L {amount:N2}", Trend = $"{count} factura(s)" });
        }
        if (Can(HotelAccessPolicies.ReadFiscalOptions))
        {
            var soon = today.AddDays(30);
            var expiring = await context.CAIs.CountAsync(row => row.Status == CAIStatus.Activo && row.DueDate <= soon, token)
                + await context.DocumentAuthorizations.CountAsync(row => row.Status == CAIStatus.Activo && row.DueDate <= soon, token);
            if (expiring > 0) summary.Alerts.Add(new()
            {
                Title = "Autorizaciones fiscales por revisar",
                Description = $"{expiring} autorización(es) vencidas o próximas a vencer requieren revisión.",
                Severity = "warning"
            });
        }
        if (Has(PermissionNames.ManageCash))
        {
            var register = await context.CashRegisters.AsNoTracking().Where(row => row.IsActive)
                .OrderBy(row => row.Name).ThenBy(row => row.Id).FirstOrDefaultAsync(token);
            if (register is not null)
            {
                var balance = await context.CashMovements.Where(row => row.CashRegisterId == register.Id)
                    .OrderByDescending(row => row.CreatedAt).ThenByDescending(row => row.Id)
                    .Select(row => (decimal?)row.BalanceAfter).FirstOrDefaultAsync(token) ?? 0m;
                summary.Cash = new() { RegisterName = register.Name, Balance = balance };
            }
        }
        if (Has(PermissionNames.ManageInventory)
            && await context.Products.AnyAsync(row => row.IsActive && row.CurrentStock <= row.MinStockLevel, token))
            summary.Alerts.Add(new() { Title = "Stock bajo", Description = "Hay productos por debajo del stock mínimo.", Severity = "danger" });
        return Ok(summary);
    }
}
