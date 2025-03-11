using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RareNFTs.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace RareNFTs.Application.DTOs;

public record CardDTO
{
    [Display(Name = "Card ID")]
    public Guid Id { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "{0} is required")]
    public string Description { get; set; } = null!;

}
