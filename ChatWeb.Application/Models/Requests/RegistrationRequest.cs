using System.ComponentModel.DataAnnotations;

namespace ChatWeb.Application.Models.Requests;

public class RegistrationRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

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
