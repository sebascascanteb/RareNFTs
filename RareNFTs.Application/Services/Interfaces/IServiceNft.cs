using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Interfaces;

public interface IServiceNft
{
    Task<ICollection<NftDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<ClientNftDTO>> ListOwnedAsync();
    Task<ICollection<NftDTO>> ListAsync();
    Task<NftDTO> FindByIdAsync(Guid id);
    Task<ClientNftDTO> FindClientNftByIdAsync(Guid id);
    Task<Guid> AddAsync(NftDTO dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, NftDTO dto);
    Task<bool> ChangeNFTOwnerAsync(Guid nftId, Guid clientId);

}
