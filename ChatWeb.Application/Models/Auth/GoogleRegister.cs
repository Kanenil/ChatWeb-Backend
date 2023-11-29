using System.ComponentModel.DataAnnotations;

namespace ChatWeb.Application.Models.Auth;

public class GoogleRegister
{
    [Required]
    public string Token { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Image { get; set; }
}
