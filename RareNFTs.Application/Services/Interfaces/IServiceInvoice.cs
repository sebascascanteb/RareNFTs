using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Interfaces;

public interface IServiceInvoice
{
    Task<Guid> AddAsync(InvoiceHeaderDTO dto);
    Task<InvoiceHeaderDTO> FindByIdAsync(Guid id);
    Task<ICollection<InvoiceHeaderDTO>> FindByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task CancelInvoiceAsync(Guid invoiceId);
    Task<ICollection<InvoiceHeaderDTO>> ListActivesAsync();
    Guid GetNewId();
}
