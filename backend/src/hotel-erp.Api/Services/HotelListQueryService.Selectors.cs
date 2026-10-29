using hotel_erp.Api.Database.Entities;
using hotel_erp.Api.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace hotel_erp.Api.Services;

public sealed partial class HotelListQueryService
{
    public Task<PagedResult<AuthorizationOptionDto>> AuthorizationsAsync(AuthorizationPageQuery request,
        DateOnly today, CancellationToken token)
    {
        var cais = context.CAIs.AsNoTracking();
        var documents = context.DocumentAuthorizations.AsNoTracking();
        if (request.ActiveOnly)
        {
            cais = cais.Where(row => row.Status == CAIStatus.Activo && row.IssueDate <= today && row.DueDate >= today);
            documents = documents.Where(row => row.Status == CAIStatus.Activo && row.IssueDate <= today && row.DueDate >= today);
        }
        if (request.Source == "cai") documents = documents.Where(_ => false);
        if (request.Source == "docauth") cais = cais.Where(_ => false);
        if (!string.IsNullOrWhiteSpace(request.DocumentType))
        {
            var type = Enum.Parse<InvoiceDocumentType>(request.DocumentType);
            if (type != InvoiceDocumentType.Factura) cais = cais.Where(_ => false);
            documents = documents.Where(row => row.DocumentType == type);
        }
        var soon = today.AddDays(30);
        var options = cais.Select(row => new AuthorizationOptionDto
        {
            Id = row.Id,
            Source = "cai",
            DocumentType = "Factura",
            CAINumber = row.CAINumber,
            IssueDate = row.IssueDate,
            DueDate = row.DueDate,
            InitialRange = row.InitialRange,
            FinalRange = row.FinalRange,
            CurrentCorrelative = row.CurrentCorrelative,
            Status = row.Status == CAIStatus.Activo ? "Activo" : row.Status == CAIStatus.Vencido ? "Vencido" : row.Status == CAIStatus.Agotado ? "Agotado" : "Desactivado",
            IsExpiringSoon = row.DueDate <= soon,
            HasAttachment = false
        }).Concat(documents.Select(row => new AuthorizationOptionDto
        {
            Id = row.Id,
            Source = "docauth",
            DocumentType = row.DocumentType == InvoiceDocumentType.Factura ? "Factura" : row.DocumentType == InvoiceDocumentType.NotaCredito ? "NotaCredito" : "NotaDebito",
            CAINumber = row.CAINumber,
            IssueDate = row.IssueDate,
            DueDate = row.DueDate,
            InitialRange = row.InitialRange,
            FinalRange = row.FinalRange,
            CurrentCorrelative = row.CurrentCorrelative,
            Status = row.Status == CAIStatus.Activo ? "Activo" : row.Status == CAIStatus.Vencido ? "Vencido" : row.Status == CAIStatus.Agotado ? "Agotado" : "Desactivado",
            IsExpiringSoon = row.DueDate <= soon,
            HasAttachment = row.AttachmentPath != null && row.AttachmentPath != ""
        }));
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            options = options.Where(row => row.CAINumber.ToLower().Contains(term));
        }
        return options.OrderByDescending(row => row.DueDate).ThenBy(row => row.Source).ThenBy(row => row.Id).ToPageAsync(request, token);
    }
}
