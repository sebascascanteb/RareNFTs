using Microsoft.EntityFrameworkCore;
using RareNFTs.Infraestructure.Data;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;

namespace RareNFTs.Infraestructure.Repository.Implementation;

public class RepositoryCountry : IRepositoryCountry
{
    private readonly RareNFTsContext _context;

    public RepositoryCountry(RareNFTsContext context)
    {
        _context = context;
    }

    //This asynchronous function adds a new Country entity to the context and saves the changes to the database. It returns the ID of the newly added country.

    public async Task<string> AddAsync(Country entity)
    {
        await _context.Set<Country>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    //This asynchronous function deletes a Country entity from the context based on the provided ID.
    //It first retrieves the entity by ID, removes it from the context, and then saves the changes to the database.

    public async Task DeleteAsync(string id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        // int rowAffected = _context.Database.ExecuteSql($"Delete Country Where IdCountry = {id}");
        // await Task.FromResult(1);

        var @object = await FindByIdAsync(id);
        _context.Remove(@object);
        _context.SaveChanges();
    }

    //This asynchronous function finds Country entities in the context by their name.
    //It retrieves a collection of countries whose names contain the specified string.

    public async Task<ICollection<Country>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Country>()
                                     .Where(p => p.Name.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    //This asynchronous function finds a Country entity in the context by its ID.
    //It retrieves the country object from the context based on the provided ID.

    public async Task<Country> FindByIdAsync(string id)
    {
        var @object = await _context.Set<Country>().FindAsync(id);

        return @object!;
    }

    //This asynchronous function retrieves a list of all Country entities from the context.
    //It returns the collection of countries without tracking changes.

    public async Task<ICollection<Country>> ListAsync()
    {
        var collection = await _context.Set<Country>().AsNoTracking().ToListAsync();
        return collection;
    }


    //This asynchronous function saves the changes made to the context to the underlying database. It commits any pending changes to the database.

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

}
