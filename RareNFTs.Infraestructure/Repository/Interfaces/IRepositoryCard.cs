using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryCard
{
    Task<ICollection<Card>> FindByDescriptionAsync(string description);
    Task<ICollection<Card>> ListAsync();
    Task<Card> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(Card entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync();

}
