using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RareNFTs.Application.Profiles;

public class CardProfile : Profile
{
    public CardProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<CardDTO, Card>().ReverseMap();
    }

}
