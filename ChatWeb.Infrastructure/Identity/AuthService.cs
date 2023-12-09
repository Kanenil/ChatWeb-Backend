using AutoMapper;
using ChatWeb.Application.Constants;
using ChatWeb.Application.Contracts.Identity;
using ChatWeb.Application.Contracts.Infrastructure;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.Exceptions;
using ChatWeb.Application.Models.Auth;
using ChatWeb.Application.Models.Requests;
using ChatWeb.Application.Models.Responses;
using ChatWeb.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace ChatWeb.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUploadService _imageService;
    private readonly IMapper _mapper;

    public AuthService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IJwtTokenService jwtTokenService, IMapper mapper, IUploadService imageService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<AuthResponse> GoogleLogin(string googleToken)
    {
        if (string.IsNullOrWhiteSpace(googleToken))
            throw new BadRequestException("Token is not valid.");


        var payload = await _jwtTokenService.VerifyGoogleTokenAsync(googleToken);

        if (payload == null)
            throw new BadRequestException("Token is long not valid.");

        string provider = "Google";
        var info = new UserLoginInfo(provider, payload.Subject, provider);
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
                throw new BadRequestException("GoogleLogin: User not found.");

            var resultUserLogin = await _userManager.AddLoginAsync(user, info);
            if (!resultUserLogin.Succeeded)
                throw new BadRequestException("Some thing went wrong, try later.");
        }

        var time = await _userManager.GetLockoutEndDateAsync(user);
        if (time != null)
            throw new BadRequestException($"Account is banned until {time.ToString()}.");

        var token = await _jwtTokenService.CreateTokenAsync(user);
        return token;
    }

    public async Task<AuthResponse> GoogleRegister(GoogleRegister model)
    {
        var payload = await _jwtTokenService.VerifyGoogleTokenAsync(model.Token);

        if (payload == null)
            throw new BadRequestException("Token is long not valid.");

        string provider = "Google";
        var info = new UserLoginInfo(provider, payload.Subject, provider);
        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new UserEntity
                {
                    Email = payload.Email,
                    UserName = model.UserName,
                    Image = await _imageService.SaveImageFromURL(model.Image)
                };

                var resultCreate = await _userManager.CreateAsync(user);
                if (!resultCreate.Succeeded)
                    throw new BadRequestException("Some thing went wrong, try later.");
                await _userManager.AddToRoleAsync(user, Roles.User);
            }

            var resultUserLogin = await _userManager.AddLoginAsync(user, info);
            if (!resultUserLogin.Succeeded)
                throw new BadRequestException("Some thing went wrong, try later.");
        }

        var token = await _jwtTokenService.CreateTokenAsync(user);
        return token;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(request.Username);

            if (user == null)
                throw new NotFoundException($"User with", request.Username);
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new BadRequestException($"Credentials for '{request.Username} aren't valid'.");
        }

        var token = await _jwtTokenService.CreateTokenAsync(user);

        return token;
    }

    public async Task<TokenModel> RefreshToken(TokenModel model)
    {
        if (model is null)
            throw new BadRequestException("Invalid client request");

        return await _jwtTokenService.RefreshTokenAsync(model.AccessToken, model.RefreshToken);
    }

    public async Task<AuthResponse> Register(RegistrationRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.UserName);

        if (existingUser != null)
            throw new BadRequestException($"User '{request.UserName}' already exists.");

        var user = _mapper.Map<UserEntity>(request);

        var result = await _userManager.CreateAsync(user, request.Password);
        await _userManager.AddToRoleAsync(user, Roles.User);

        if (!result.Succeeded)
            throw new BadRequestException($"{result.Errors}");

        var token = await _jwtTokenService.CreateTokenAsync(user);

        return token;
    }
}
