using System.ComponentModel.DataAnnotations;

namespace Dto;

public class LoginDto
{
    [Required(ErrorMessage = "E-Mailadresse wird benötigt!")]
    [EmailAddress(ErrorMessage = "E-Mail hat nicht das richtige Format!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Passwort wird benötigt!")]
    public string? Password { get; set; }
}