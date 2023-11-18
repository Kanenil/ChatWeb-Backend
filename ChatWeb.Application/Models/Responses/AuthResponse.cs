using ChatWeb.Application.Models.Auth;

namespace ChatWeb.Application.Models.Responses;

public class AuthResponse
{
    public TokenModel Tokens { get; set; }
    public DateTime Expires { get; set; }
}
