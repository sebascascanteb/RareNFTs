using RareNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Interfaces;

public interface IServiceClient
{
    Task<ICollection<ClientDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<ClientDTO>> ListAsync();
    Task<ICollection<ClientDTO>> ListOwnersAsync();
    Task<ClientDTO> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(ClientDTO dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, ClientDTO dto);
    Task<IEnumerable<ClientNftDTO>> FindByNftNameAsync(string name);

}
