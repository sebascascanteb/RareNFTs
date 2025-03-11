using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace RareNFTs.Application.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<RoleDTO, Role>().ReverseMap();
    }
}
