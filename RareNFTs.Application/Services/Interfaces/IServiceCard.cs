using RareNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Interfaces;

public interface IServiceCard
{
    Task<ICollection<CardDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<CardDTO>> ListAsync();
    Task<CardDTO> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(CardDTO dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, CardDTO dto);
}
