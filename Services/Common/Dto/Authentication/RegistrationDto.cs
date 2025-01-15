using System.ComponentModel.DataAnnotations;

namespace Dto;

public class RegistrationDto
{
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Email { get; set; }

    [Required(ErrorMessage = "Pflichtfeld")]
    [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*",
    ErrorMessage = "Das Passwort muss mindestens 7 Zeichen lang sein und mindestens einen Großbuchstaben und eine Zahl enthalten!")]
    [StringLength(16, MinimumLength = 7, ErrorMessage = "Mindestens 7")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Pflichtfeld")]
    [Compare(nameof(Password), ErrorMessage = "Passwörter stimmen nicht überein")]
    public string ConfirmPassword { get; set; } = null!;

    public string? MobileNumber { get; set; } = null!;
    public string? Role { get; set; }
}