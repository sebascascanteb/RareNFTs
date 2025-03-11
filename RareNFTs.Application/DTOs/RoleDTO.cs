using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.DTOs;

public record RoleDTO
{
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Code")]
    public Guid Id { get; set; }

    [Display(Name = "Role Description")]
    [Required(ErrorMessage = "{0} is required")]
    public string Description { get; set; } = null!;

}
