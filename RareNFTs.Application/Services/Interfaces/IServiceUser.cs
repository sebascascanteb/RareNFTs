using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Interfaces;

public interface IServiceUser
{
    Task<ICollection<UserDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<UserDTO>> ListAsync();
    Task<ICollection<RoleDTO>> ListRoleAsync();
    Task<UserDTO> FindByIdAsync(Guid id);
    Task<UserDTO> LoginAsync(string email, string password);
    Task<string> AddAsync(UserDTO dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, UserDTO dto);

}
