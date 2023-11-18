using ChatWeb.Application.Models.Auth;
using ChatWeb.Application.Models.Requests;
using ChatWeb.Application.Models.Responses;

namespace ChatWeb.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<AuthResponse> Register(RegistrationRequest request);
    Task<AuthResponse> GoogleLogin(string googleToken);
    Task<AuthResponse> GoogleRegister(GoogleRegister model);
    Task<TokenModel> RefreshToken(TokenModel model);
}
