using RareNFTs.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Services.Interfaces;

public interface IServiceCountry
{
    Task<ICollection<CountryDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<CountryDTO>> ListAsync();
    Task<CountryDTO> FindByIdAsync(string id);
    Task<string> AddAsync(CountryDTO dto);
    Task DeleteAsync(string id);
    Task UpdateAsync(string id, CountryDTO dto);
}
