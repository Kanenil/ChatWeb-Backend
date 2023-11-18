using ChatWeb.Application.Models.Auth;
using ChatWeb.Application.Models.Responses;
using ChatWeb.Domain.Identity;
using Google.Apis.Auth;

namespace ChatWeb.Application.Contracts.Infrastructure;

public interface IJwtTokenService
{
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string tokenId);
    Task<AuthResponse> CreateTokenAsync(UserEntity user);
    Task<TokenModel> RefreshTokenAsync(string? accessToken, string? refreshToken);
}
