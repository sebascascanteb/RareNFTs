using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.DTOs;

public record InvoiceDetailDTO
{
    [Display(Name = "Invoice Code")]
    public Guid IdInvoice { get; set; }

    [Display(Name = "Nft Code")]
    public Guid IdNft { get; set; }

    [Display(Name = "Nft Name")]
    public string NftDescription { get; set; } = default!;

    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "Price")]
    public decimal? Price { get; set; }

    [Display(Name = "Quantity")]
    public int? Quantity { get; set; }
    
    [Display(Name = "Sequence")]
    public int Sequence { get; set; }

    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "Total")]
    public decimal? TotalLine { get; set; }
}
