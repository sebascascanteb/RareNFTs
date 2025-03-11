using System.ComponentModel.DataAnnotations;

namespace RareNFTs.Web.ViewModels;
 
  
public record ViewModelInput
{
    [Display(Name = "Nft")]
    public int Id { get; set; }

    [Display(Name = "Quantity")]
    [Range(0, 999999999, ErrorMessage = "Minimum quantity {1}")]
    public int Quantity { get; set; }
    [Range(0, 999999999, ErrorMessage = "Minimum price {1}")]    
    [Display(Name = "Price")]
    public decimal Price { get; set; }

}