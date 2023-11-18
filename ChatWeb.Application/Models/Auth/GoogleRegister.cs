using System.ComponentModel.DataAnnotations;

namespace ChatWeb.Application.Models.Auth;

public class GoogleRegister
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Image { get; set; }
}
