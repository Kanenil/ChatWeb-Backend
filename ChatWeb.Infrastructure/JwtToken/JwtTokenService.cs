using ChatWeb.Application.Contracts.Infrastructure;
using ChatWeb.Application.Exceptions;
using ChatWeb.Application.Models.Auth;
using ChatWeb.Application.Models.Responses;
using ChatWeb.Domain.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatWeb.Infrastructure.JwtToken;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;
    private readonly UserManager<UserEntity> _userManager;

    public JwtTokenService(IConfiguration config, UserManager<UserEntity> userManager)
    {
        _config = config;
        _userManager = userManager;
    }
    public async Task<AuthResponse> CreateTokenAsync(UserEntity user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        List<Claim> claims = new()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

        foreach (var userRole in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = CreateToken(claims);
        var refreshToken = GenerateRefreshToken();

        _ = int.TryParse(_config["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        var refreshExpires = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).AddDays(refreshTokenValidityInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshExpires;

        await _userManager.UpdateAsync(user);

        return new()
        {
            Tokens = new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            },
            Expires = refreshExpires
        };
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string tokenId)
    {
        string clientID = _config["GoogleAuthSettings:ClientId"];

        var setting = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { clientID }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(tokenId, setting);
        return payload;
    }

    public async Task<TokenModel> RefreshTokenAsync(string? accessToken, string? refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
            throw new UnAuthorizedExeption("Invalid access token or refresh token");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            throw new UnAuthorizedExeption("Invalid access token or refresh token");

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
        _ = int.TryParse(_config["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: _config["JWT:ValidIssuer"],
            audience: _config["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
