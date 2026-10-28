using hotel_erp.Api.Database;
using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public interface ICaiCorrelativeService
{
    Task<string> GetNextAsync(Guid caiId);
}

public sealed class CaiCorrelativeService(
    ApplicationDbContext context,
    IUnitOfWork unitOfWork,
    IHotelClock clock) : ICaiCorrelativeService
{
    public async Task<string> GetNextAsync(Guid caiId)
    {
        return await PostgresCorrelativeLock.ExecuteAsync(context, clock, $"cai:{caiId}", async () =>
        {
            var cai = await context.CAIs.FirstOrDefaultAsync(c => c.Id == caiId) ?? throw new InvalidOperationException("CAI no encontrado");
            if (cai.Status != CAIStatus.Activo) throw new InvalidOperationException("El CAI no está activo");
            if (cai.DueDate < clock.Today) throw new InvalidOperationException("El CAI está vencido");

            var parts = cai.CurrentCorrelative.Split('-');
            if (parts.Length != 4) throw new InvalidOperationException("Formato de correlativo inválido");

            var hasIssuedDocuments = await context.Invoices
                .IgnoreQueryFilters()
                .AnyAsync(invoice => invoice.CAIId == caiId);
            var sequential = !hasIssuedDocuments && cai.CurrentCorrelative == cai.InitialRange
                ? int.Parse(parts[3])
                : int.Parse(parts[3]) + 1;
            var finalSeq = int.Parse(cai.FinalRange.Split('-').Last());
            if (sequential > finalSeq)
            {
                cai.Status = CAIStatus.Agotado;
                await unitOfWork.SaveChangesAsync();
                throw new CorrelativeStateException("El CAI ha agotado su rango de correlativos");
            }

            var newCorrelative = $"{parts[0]}-{parts[1]}-{parts[2]}-{sequential:D8}";

            cai.CurrentCorrelative = newCorrelative;
            context.CAIs.Update(cai);
            await unitOfWork.SaveChangesAsync();
            return newCorrelative;
        });
    }
}
