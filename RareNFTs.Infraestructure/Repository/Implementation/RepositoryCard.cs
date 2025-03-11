using Microsoft.EntityFrameworkCore;
using RareNFTs.Infraestructure.Data;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;

namespace RareNFTs.Infraestructure.Repository.Implementation;

public class RepositoryCard : IRepositoryCard
{
    private readonly RareNFTsContext _context;

    public RepositoryCard(RareNFTsContext context)
    {
        _context = context;
    }

    //This asynchronous function adds a new Card entity to the context and saves the changes to the database. It returns the ID of the newly added card.

    public async Task<Guid> AddAsync(Card entity)
    {
        await _context.Set<Card>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }


    //This asynchronous function deletes a Card entity from the context based on the provided ID.
    //It first retrieves the entity by ID, removes it from the context, and then saves the changes to the database.

    public async Task DeleteAsync(Guid id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        // int rowAffected = _context.Database.ExecuteSql($"Delete Card Where IdCard = {id}");
        // await Task.FromResult(1);

        var @object = await FindByIdAsync(id);
        _context.Remove(@object);
        _context.SaveChanges();
    }

    //This asynchronous function finds Card entities in the context by their description. It retrieves a collection of cards whose descriptions contain the specified string.

    public async Task<ICollection<Card>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Card>()
                                     .Where(p => p.Description.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    //This asynchronous function finds a Card entity in the context by its ID. It retrieves the card object from the context based on the provided ID.

    public async Task<Card> FindByIdAsync(Guid id)
    {
        var @object = await _context.Set<Card>().FindAsync(id);

        return @object!;
    }

    //This asynchronous function retrieves a list of all Card entities from the context. It returns the collection of cards without tracking changes.

    public async Task<ICollection<Card>> ListAsync()
    {
        var collection = await _context.Set<Card>().AsNoTracking().ToListAsync();
        return collection;
    }

    //This asynchronous function saves the changes made to the context to the underlying database. It commits any pending changes to the database.

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

}
