using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RareNFTs.Application.Profiles;

public class CountryProfile : Profile
{
    public CountryProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<CountryDTO, Country>().ReverseMap();
    }

}
