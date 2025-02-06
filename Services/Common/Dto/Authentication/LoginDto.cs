using System.ComponentModel.DataAnnotations;

namespace Dto;

public class LoginDto
{
    [Required(ErrorMessage = "E-Mail Address is required")]
    [EmailAddress(ErrorMessage = "E-Mail is not correctly formated")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}