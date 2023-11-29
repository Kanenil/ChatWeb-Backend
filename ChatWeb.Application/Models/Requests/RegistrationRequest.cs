using System.ComponentModel.DataAnnotations;

namespace ChatWeb.Application.Models.Requests;

public class RegistrationRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [MinLength(6)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
