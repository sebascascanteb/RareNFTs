using Microsoft.EntityFrameworkCore;
using RareNFTs.Infraestructure.Data;
using RareNFTs.Infraestructure.Models;
using RareNFTs.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Infraestructure.Repository.Implementation;

public class RepositoryUser : IRepositoryUser
{
    private readonly RareNFTsContext _context;

    public RepositoryUser(RareNFTsContext context)
    {
        _context = context;
    }

    //This asynchronous function adds a new user entity to the database. It saves changes to the context and returns the email of the added user.

    public async Task<string> AddAsync(User entity)
    {
        await _context.Set<User>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Email!;
    }

    //This asynchronous function deletes a user entity from the database based on its ID. It removes the entity from the context and saves the changes.

    public async Task DeleteAsync(Guid id)
    {

        var @object = await FindByIdAsync(id);
        _context.Remove(@object);
        _context.SaveChanges();
    }

    //This asynchronous function retrieves a collection of user entities from the database based on a partial match of their email addresses.

    public async Task<ICollection<User>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<User>()
                                     .Where(p => p.Email!.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    //This asynchronous function retrieves a user entity from the database based on its ID. It includes the related role entity and operates in a read-only mode.

    public async Task<User> FindByIdAsync(Guid id)
    {
        var @object = await _context.Set<User>()
                                       .Include(b => b.IdRoleNavigation)
                                       .AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        return @object!;
    }

    //This asynchronous function retrieves a collection of all user entities from the database. It includes the related role entity and operates in a read-only mode.

    public async Task<ICollection<User>> ListAsync()
    {
        var collection = await _context.Set<User>()
                                       .Include(b => b.IdRoleNavigation)
                                       .AsNoTracking().ToListAsync();
        return collection;
    }

    //This asynchronous function retrieves a collection of all role entities from the database.

    public async Task<ICollection<Role>> ListRoleAsync()
    {
        var collection = await _context.Set<Role>().ToListAsync();
        return collection;
    }

    //This asynchronous function authenticates a user based on their email and password. It retrieves the user entity with the specified email and password from the database, including the related role entity.

    public async Task<User> LoginAsync(string email, string password)
    {
        var @object = await _context.Set<User>()
                                    .Include(b => b.IdRoleNavigation)
                                    .Where(p => p.Email == email && p.Password == password)
                                    .FirstOrDefaultAsync();
        return @object!;
    }

    //This asynchronous function saves any changes made to user entities in the context to the database.

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }
}
