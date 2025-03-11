using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Profiles;

public class UserProfile: Profile 
{
    public UserProfile() {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<UserDTO, User>();

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Id, orig => orig.MapFrom(x => x.Id))
            .ForMember(dest => dest.Email, orig => orig.MapFrom(x => x.Email))
            .ForMember(dest => dest.Password, orig => orig.MapFrom(x => x.Password))
            .ForMember(dest => dest.IdRole, orig => orig.MapFrom(x => x.IdRole))
            .ForMember(dest => dest.DescriptionRole, orig => orig.MapFrom(x => x.IdRoleNavigation.Description));
    }
}
