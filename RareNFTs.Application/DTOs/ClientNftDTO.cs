using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.DTOs;

public record  ClientNftDTO


{

    [Display(Name = "Client Id")]

    public Guid IdClient { get; set; }
   

    [Display(Name = "Nft Id")]

    public Guid IdNft { get; set; }
    [Display(Name = "Date")]

    public DateTime? Date { get; set; }

}
