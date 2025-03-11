using RareNFTs.Application.DTOs;
using RareNFTs.Infraestructure.Models;
using System.ComponentModel.DataAnnotations;

namespace RareNFTs.Web.ViewModels;

public class ViewModelClientNft
{
    [Display(Name = "Id Owner")]
    public Guid IdClient { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Name")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Surname")]
    public string Surname { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Genre")]
    public string Genre { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Country")]
    public string IdCountry { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Email")]
    public string Email { get; set; } = default!;

    [Display(Name = "Id NFT")]
    [Required]
    public Guid IdNft { get; set; }

    [Required]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required]
    [Range(0, 999999999, ErrorMessage = "The minimun price is {1} and the maximum {2}")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:######.00}")]
    [Display(Name = "Price")]
    public decimal? Price { get; set; }

    [Display(Name = "Image")]
    [Required]
    public byte[] Image { get; set; } = null!;

    [Display(Name = "Date ownership")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Date { get; set; } = DateTime.Now;

    [Required]
    [Display(Name = "Author")]
    public string? Author { get; set; }
}
