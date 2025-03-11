using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryClient
{
    Task<ICollection<Client>> FindByDescriptionAsync(string description);
    Task<ICollection<Client>> ListAsync();
    Task<ICollection<Client>> ListOwnersAsync();
    Task<Client> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(Client entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync();
    Task<IEnumerable<ClientNft>> FindByNftNameAsync(string name);

}
