namespace BlazorECommerce.Shared.UAC;

using System.ComponentModel.DataAnnotations;

public class UserRegister
{
    [Required, EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}