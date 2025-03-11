using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Profiles;

public class ClientNftProfile : Profile
{
    public ClientNftProfile()
    {
        CreateMap<ClientNft, ClientNftDTO>()
            .ForMember(dest => dest.IdClient, opt => opt.MapFrom(src => src.IdClient))
            .ForMember(dest => dest.IdNft, opt => opt.MapFrom(src => src.IdNft))
                    .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

    }
}
