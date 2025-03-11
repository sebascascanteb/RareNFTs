using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Infraestructure.Repository.Interfaces;

public interface IRepositoryNft
{
    Task<ICollection<Nft>> FindByDescriptionAsync(string description);
    Task<ICollection<ClientNft>> ListOwnedAsync();
    Task<ICollection<Nft>> ListAsync();
    Task<Nft> FindByIdAsync(Guid id);
    Task<ClientNft> FindClientNftByIdAsync(Guid id);
    Task<Guid> AddAsync(Nft entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync();
    Task<bool> ChangeNFTOwnerAsync(Guid nftId, Guid clientId);

}
