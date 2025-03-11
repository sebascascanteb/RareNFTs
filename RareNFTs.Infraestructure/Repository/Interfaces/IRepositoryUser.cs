using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryUser
{
    Task<ICollection<User>> FindByDescriptionAsync(string description);
    Task<ICollection<User>> ListAsync();
    Task<ICollection<Role>> ListRoleAsync();
    Task<User> FindByIdAsync(Guid id);

    Task<User> LoginAsync(string email, string password);
    Task<string> AddAsync(User entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync();
}
