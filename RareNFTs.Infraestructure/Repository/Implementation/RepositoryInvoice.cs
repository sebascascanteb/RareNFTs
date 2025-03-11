using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RareNFTs.Infraestructure.Data;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RareNFTs.Infraestructure.Repository.Implementation;

public class RepositoryInvoice : IRepositoryInvoice
{
    private readonly RareNFTsContext _context;

    public RepositoryInvoice(RareNFTsContext context)
    {
        _context = context;

    }

    //This asynchronous function adds a new invoice header to the database along with
    //associated invoice details. It begins a transaction, adds the invoice header, updates the client's NFT
    //ownership, adjusts the NFT inventory, and commits the transaction.

    public async Task<Guid> AddAsync(InvoiceHeader entity)
    {

        try
        {
            // Begin Transaction
            await _context.Database.BeginTransactionAsync();

            // Add InvoiceHeader to database
            await _context.Set<InvoiceHeader>().AddAsync(entity);
            await _context.SaveChangesAsync();

            // Add entries to ClientNFT table for each NFT acquired in the invoice
            foreach (var item in entity.InvoiceDetail)
            {
                // Add entry to ClientNFT table
                await _context.Set<ClientNft>().AddAsync(new ClientNft
                {
                    IdClient = entity.IdClient,
                    IdNft = item.IdNft,
                    Date = DateTime.Now
                });
            }

            // Update inventory
            foreach (var item in entity.InvoiceDetail)
            {
                var ProductNft = await _context.Set<Nft>().FindAsync(item.IdNft);
                ProductNft!.Quantity -= item.Quantity;
                _context.Set<Nft>().Update(ProductNft);
            }

            await _context.SaveChangesAsync();

            // Commit Transaction
            await _context.Database.CommitTransactionAsync();

            return entity.Id;
        }
        catch (Exception ex)
        {
            // Rollback Transaction
            await _context.Database.RollbackTransactionAsync();
            throw new Exception("An error occurred while processing the transaction.", ex);
        }
    }

    //This asynchronous function retrieves an invoice header from the database by its ID. It includes related invoice details and client information.

    public async Task<InvoiceHeader> FindByIdAsync(Guid id)
    {

        var response = await _context.Set<InvoiceHeader>()
                    .Include(detail => detail.InvoiceDetail)
                    .ThenInclude(detail => detail.IdNftNavigation)
                    .Include(client => client.IdClientNavigation)
        .Where(p => p.Id == id).FirstOrDefaultAsync();
        return response!;
    }


    //This asynchronous function retrieves a list of invoice headers within the specified date range from the database.
    //It includes related invoice details and client information.

    public async Task<List<InvoiceHeader>> FindByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var invoices = await _context.Set<InvoiceHeader>()
        .Include(detail => detail.InvoiceDetail)
        .ThenInclude(detail => detail.IdNftNavigation)
        .Include(client => client.IdClientNavigation)
            .Where(p => p.Date >= startDate && p.Date <= endDate)
            .ToListAsync();

        return invoices;
    }


    //This asynchronous function cancels an invoice by changing its status to false.
    //It removes associated entries from the ClientNFT table, adjusts NFT quantities, and commits the transaction.

    public async Task CancelInvoiceAsync(Guid invoiceId)
    {
        // Begin Transaction
        var transaction = _context.Database.BeginTransactionAsync().Result;

        try
        {

            var invoice = FindByIdAsync(invoiceId).Result;

            if (invoice == null)
            {
                throw new Exception("An error occurred while canceling the invoice. Invoice not found.");
            }

            // Change the status of the invoice to false
            invoice.Status = false;

            // Remove entries from ClientNFT table
            foreach (var detail in invoice.InvoiceDetail.ToList())
            {
                // Remove entries from ClientNFT table
                var clientNFTs = (from clientNft in _context.ClientNft
                                  join client in _context.Client on clientNft.IdClient equals client.Id
                                  join nft in _context.Nft on clientNft.IdNft equals nft.Id
                                  where nft.Id == detail.IdNft
                                  select new ClientNft
                                  {
                                      IdClient = clientNft.IdClient,
                                      IdNft = clientNft.IdNft,
                                      Date = clientNft.Date
                                  })
                         .Distinct()
                         .ToList();

                _context.Set<ClientNft>().RemoveRange(clientNFTs);

                // Increment the Quantity of NFT by 1
                var oNft = _context.Set<Nft>().FindAsync(detail.IdNft).Result;
                if (oNft != null)
                {
                    oNft.Quantity += 1;
                    _context.Set<Nft>().Update(oNft);
                }
            }

            _context.SaveChanges();

            // Commit Transaction
            _context.Database.CommitTransaction();

        }
        catch (Exception ex)
        {
            // Rollback Transaction
            _context.Database.RollbackTransaction();
            throw new Exception("An error occurred while canceling the invoice.", ex);
        }
    }


   // This asynchronous function retrieves a collection of active invoice headers from the database, where the status is true.

    public async Task<ICollection<InvoiceHeader>> ListActivesAsync()
    {
        try
        {
            // Consultar los InvoiceHeaders con Status igual a true
            var activeInvoiceHeaders = await _context.Set<InvoiceHeader>()
                .Where(ih => ih.Status)
                .ToListAsync();

            return activeInvoiceHeaders!;
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que pueda ocurrir
            throw new Exception("An error occurred while retrieving active InvoiceHeaders.", ex);
        }
    }
}
