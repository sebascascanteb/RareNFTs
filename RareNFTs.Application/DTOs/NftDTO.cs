using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.DTOs;

public record NftDTO
{
    [Display(Name = "Id")]
    [Required]
    public Guid Id { get; set; }

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

    [Display(Name = "Date")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    [Display(Name = "Author")]
    public string? Author { get; set; }

    [Display(Name = "Quantity")]
    [Range(0, 1, ErrorMessage = "The minimum value is {1} and the maximum {2}")]
    [Required]
    public int? Quantity { get; set; }



}
