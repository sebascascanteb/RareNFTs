using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RareNFTs.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace RareNFTs.Web.ViewModels;
 
  
public record ViewModelInvoice
{
    [Display(Name = "Invoice ID")]
    [ValidateNever]
    public Guid Id { get; set; }

    [Display(Name = "Card")]
    [Required(ErrorMessage = "{0} is required")]
    public Guid IdCard { get; set; }

    [Display(Name = "Client")]
    [Required(ErrorMessage = "{0} is required")]
    public Guid IdClient { get; set; }

    [Display(Name = "Date")]
    [Required(ErrorMessage = "{0} is required")]
    public DateTime? Date { get; set; }

    [Display(Name = "Status")]
    public int Status { get; set; }

    [Display(Name = "Card Number")]
    [Required(ErrorMessage = "{0} is required")]
    public int? NumCard { get; set; }

    [Display(Name = "Total")]
    [Required(ErrorMessage = "{0} is required")]
    public decimal? Total { get; set; }

    public List<InvoiceDetailDTO> ListInvoiceDetail = null!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Name")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Surname")]
    public string Surname { get; set; } = default!;

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Email")]
    public string Email { get; set; } = default!;

    [Display(Name = "Card Brand")]
    [Required(ErrorMessage = "{0} is required")]
    public string CardDescription { get; set; } = null!;

}