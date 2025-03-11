using RareNFTs.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RareNFTs.Application.DTOs;

public record class UserDTO
{

    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]

    public string? Email { get; set; }

    [Required]
    [Display(Name = "Password")]
    public string? Password { get; set; }

    [Required]
    public Guid IdRole { get; set; }

    [Display(Name = "Role")]
    public string? DescriptionRole { get; set; } = default;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
