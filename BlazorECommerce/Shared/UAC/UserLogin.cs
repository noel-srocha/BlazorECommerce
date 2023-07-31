namespace BlazorECommerce.Shared.UAC;

using System.ComponentModel.DataAnnotations;

public class UserLogin
{
    [Required, EmailAddress]
    public string EmailAddress { get; set; }
    [Required]
    public string Password { get; set; }
}