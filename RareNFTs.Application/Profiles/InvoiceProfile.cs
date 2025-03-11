using AutoMapper;
using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.Profiles;


public class InvoiceProfile : Profile
{

    public InvoiceProfile()
    {
        CreateMap<InvoiceHeaderDTO, InvoiceHeader>().ReverseMap();
        CreateMap<InvoiceDetailDTO, InvoiceDetail>().ReverseMap();

        CreateMap<InvoiceHeaderDTO, InvoiceHeader>()
                 .ForMember(dest => dest.Id, orig => orig.MapFrom(x => x.Id))
                 .ForMember(dest => dest.IdCard, orig => orig.MapFrom(x => x.IdCard))
                 .ForMember(dest => dest.IdClient, orig => orig.MapFrom(x => x.IdClient))
                 .ForMember(dest => dest.InvoiceDetail, orig => orig.MapFrom(x => x.ListInvoiceDetail));
    }
}
