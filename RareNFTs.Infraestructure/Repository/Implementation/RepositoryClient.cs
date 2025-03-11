using Microsoft.EntityFrameworkCore;
using RareNFTs.Infraestructure.Data;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;


namespace RareNFTs.Infraestructure.Repository.Implementation;

public class RepositoryClient : IRepositoryClient
{

    private readonly RareNFTsContext _context;

    public RepositoryClient(RareNFTsContext context)
    {
        _context = context;
    }

    //This asynchronous function adds a new Client entity to the context and saves the changes to the database. It returns the ID of the newly added client.

    public async Task<Guid> AddAsync(Client entity)
    {
        await _context.Set<Client>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    //This asynchronous function deletes a Client entity from the context based on the provided ID.
    //It first retrieves the entity by ID, removes it from the context, and then saves the changes to the database.

    public async Task DeleteAsync(Guid id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        // int rowAffected = _context.Database.ExecuteSql($"Delete Client Where IdClient = {id}");
        // await Task.FromResult(1);

        var @object = await FindByIdAsync(id);
        _context.Remove(@object);
        _context.SaveChanges();
    }

    //This asynchronous function deletes a Client entity from the context based on
    //the provided ID. It first retrieves the entity by ID, removes it from the context, and then saves the changes to the database.

    public async Task<ICollection<Client>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Client>()
                                     .Where(p => p.Name.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    //This asynchronous function finds a Client entity in the context by its ID. It retrieves the client object from the context based on the provided ID.

    public async Task<Client> FindByIdAsync(Guid id)
    {
        var @object = await _context.Set<Client>().FindAsync(id);

        return @object!;
    }

    //This asynchronous function finds Client entities associated with a given NFT name. It retrieves a collection of clients who own the NFTs with names containing the specified string.

    public async Task<IEnumerable<ClientNft>> FindByNftNameAsync(string name)
    {
        var result = await (from clientNft in _context.ClientNft
                            join client in _context.Client on clientNft.IdClient equals client.Id
                            join nft in _context.Nft on clientNft.IdNft equals nft.Id
                            where nft.Description.ToLower().Contains(name.ToLower())
                            select new ClientNft
                            {
                                IdClient = clientNft.IdClient,
                                IdNft = clientNft.IdNft
                            })
                         .Distinct()
                         .ToListAsync();

        return result;
    }

    //This asynchronous function retrieves a list of all Client entities from the context. It returns the collection of clients without tracking changes.

    public async Task<ICollection<Client>> ListAsync()
    {
        var collection = await _context.Set<Client>().AsNoTracking().ToListAsync();
        return collection;
    }

    //This asynchronous function retrieves a list of clients who are owners of one or more NFTs. It queries the database to find clients associated with at least one ClientNft entity and returns the collection of owners.

    public async Task<ICollection<Client>> ListOwnersAsync()
    {
        try
        {
            // Query to get clients who are owners of one or more NFTs
            var owners = await _context.Set<Client>()
                .Where(c => _context.Set<ClientNft>().Any(cn => cn.IdClient == c.Id))
                .ToListAsync();

            return owners;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the list of NFT owners.", ex);
        }        
    }

    //This asynchronous function saves the changes made to the context to the underlying database. It commits any pending changes to the database.

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }
}
