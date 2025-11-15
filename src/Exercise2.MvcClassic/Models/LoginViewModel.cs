using System.ComponentModel.DataAnnotations;

namespace Exercise2.MvcClassic.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "User name")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
