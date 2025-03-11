using Microsoft.EntityFrameworkCore;
using RareNFTs.Infraestructure.Data;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RareNFTs.Infraestructure.Repository.Implementation;

public class RepositoryNft : IRepositoryNft
{
    private readonly RareNFTsContext _context;

    public RepositoryNft(RareNFTsContext context)
    {
        _context = context;
    }


    //This asynchronous function adds a new NFT entity to the database. It saves changes to the context and returns the ID of the added entity.

    public async Task<Guid> AddAsync(Nft entity)
    {
        await _context.Set<Nft>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }


    //This asynchronous function deletes an NFT entity from the database based on its ID. It removes the entity from the context and saves the changes.

    public async Task DeleteAsync(Guid id)
    {
        var @object = await FindByIdAsync(id);
        _context.Remove(@object);
        _context.SaveChanges();
    }

   // This asynchronous function retrieves a collection of NFT entities from the database based on a partial match of their descriptions.

    public async Task<ICollection<Nft>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Nft>()
                                     .Where(p => p.Description.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    //These asynchronous functions retrieve an NFT entity from the database based on its ID. The first overload accepts an integer ID, while the second overload accepts a GUID ID.

    public async Task<Nft> FindByIdAsync(int id)
    {
        var @object = await _context.Set<Nft>().FindAsync(id);
        return @object!;
    }

    //This asynchronous function retrieves a collection of NFT entities from the database. It includes related ClientNft entries and operates in a read-only mode.

    public async Task<Nft> FindByIdAsync(Guid id)
    {
        var @object = await _context.Set<Nft>().FindAsync(id);
        return @object!;
    }


    //This asynchronous function saves any changes made to NFT entities in the context to the database.

    public async Task<ICollection<Nft>> ListAsync()
    {
        var collection = await _context.Set<Nft>().
                                            Include(b => b.ClientNft).
                                            AsNoTracking().ToListAsync();
        return collection;
    }

    //This asynchronous function saves any changes made to NFT entities in the context to the database.

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    //This asynchronous function changes the owner of an NFT. It begins a transaction,
    //removes the previous owner's ClientNft entries, adds a new entry for the new owner, and commits the transaction.

    public async Task<bool> ChangeNFTOwnerAsync(Guid nftId, Guid clientId)
    {
        try
        {
            // Begin Transaction
            await _context.Database.BeginTransactionAsync();

            // Find the NFT
            var nft = await _context.Set<Nft>().FindAsync(nftId);

            if (nft == null)
            {
                // NFT not found
                throw new Exception("An error occurred while changing the NFT owner. NFT not found.");
            }

            // Find the previous owner's ClientNFT entries
            var previousOwnerEntries = await _context.Set<ClientNft>()
                .Where(cn => cn.IdNft == nftId)
                .ToListAsync();

            // Remove previous owner's entries from ClientNFT table
            _context.Set<ClientNft>().RemoveRange(previousOwnerEntries);

            // Create new ClientNFT entry for the new owner
            await _context.Set<ClientNft>().AddAsync(new ClientNft
            {
                IdClient = clientId,
                IdNft = nftId,
                Date = DateTime.Now // date of ownership
            });

            // Save changes
            await _context.SaveChangesAsync();

            // Commit Transaction
            await _context.Database.CommitTransactionAsync();

            return true;
        }
        catch (Exception ex)
        {
            // Rollback Transaction
            await _context.Database.RollbackTransactionAsync();
            throw new Exception("An error occurred while changing the NFT owner.", ex);
        }
    }


    //This asynchronous function retrieves a collection of all ClientNft entries from the database. It includes related NFT and Client entities.

    public async Task<ICollection<ClientNft>> ListOwnedAsync()
    {
        try
        {
            var clientNfts = await _context.Set<ClientNft>()
                .Include(cn => cn.IdNftNavigation)
                .Include(cn => cn.IdClientNavigation) 
                .ToListAsync();

            return clientNfts;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all ClientNfts from the database.", ex);
        }
    }


    //This asynchronous function retrieves a ClientNft entry from the database based on either its Client ID or NFT ID. It includes related NFT and Client entities and handles exceptions.

    public async Task<ClientNft> FindClientNftByIdAsync(Guid id)
    {
        try
        {
            var clientNft = await _context.Set<ClientNft>()
                .Include(cn => cn.IdNftNavigation) 
                .Include(cn => cn.IdClientNavigation) 
                .FirstOrDefaultAsync(cn => cn.IdClient == id || cn.IdNft == id);

            if (clientNft == null)
                throw new Exception("ClientNft not found.");

            return clientNft;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the ClientNft.", ex);
        }
    }
}
