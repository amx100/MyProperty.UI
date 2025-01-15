using System.ComponentModel.DataAnnotations;

namespace Dto;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Passwort wird benötigt.")]
    public string? OldPassword { get; set; }

    [Required(ErrorMessage = "Passwort wird benötigt.")]
    [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$",
        ErrorMessage =
            "Das Passwort muss zwischen 8 und 16 Zeichen lang sein und mindestens einen Großbuchstaben, einen Kleinbuchstaben, eine Zahl und ein Symbol enthalten.")]
    [StringLength(16, MinimumLength = 7, ErrorMessage = "Mindestens 8")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "Bitte bestätigen Sie das Passwort.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Die Passwörter müssen übereinstimmen.")]
    public string? ConfirmPassword { get; set; }

    public string? Email { get; set; }
}