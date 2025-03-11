using System.ComponentModel.DataAnnotations;

namespace RareNFTs.Web.ViewModels;

public record ViewModelLogin
{
    [Display(Name = "User")]
    [Required(ErrorMessage = "{0} is required")]
    public string User { get; set; } = default!;
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Password Length Policy Error")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only numbers and letters")]
    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Password")]
    public string Password { get; set; } = default!;
}

