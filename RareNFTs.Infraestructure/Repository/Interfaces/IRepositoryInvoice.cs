using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryInvoice
{
    Task<Guid> AddAsync(InvoiceHeader entity);
    Task<InvoiceHeader> FindByIdAsync(Guid id);
    Task<List<InvoiceHeader>> FindByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task CancelInvoiceAsync(Guid invoiceId);
    Task<ICollection<InvoiceHeader>> ListActivesAsync();
}
