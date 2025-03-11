using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Profiles;

public class NftProfile : Profile
{
    public NftProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<NftDTO, Nft>().ReverseMap();
    }
}
