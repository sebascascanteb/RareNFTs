using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryCountry
{
    Task<ICollection<Country>> FindByDescriptionAsync(string description);
    Task<ICollection<Country>> ListAsync();
    Task<Country> FindByIdAsync(string id);
    Task<string> AddAsync(Country entity);
    Task DeleteAsync(string id);
    Task UpdateAsync();

}
