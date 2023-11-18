using ChatWeb.API.Middleware;
using ChatWeb.Application.Contracts.Identity;
using ChatWeb.Application.Models.Auth;
using ChatWeb.Application.Models.Requests;
using ChatWeb.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChatWeb.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;

    public AuthController(IAuthService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    // POST api/<AuthController>/login
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        return Ok(await _authenticationService.Login(request));
    }

    // POST api/<AuthController>/register
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<AuthResponse>> Register(RegistrationRequest request)
    {
        return Ok(await _authenticationService.Register(request));
    }

    // POST api/<AuthController>/google/login
    [HttpPost("google/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<AuthResponse>> GoogleLogin(GoogleLogin model)
    {
        return Ok(await _authenticationService.GoogleLogin(model.GoogleToken));
    }

    // POST api/<AuthController>/google/register
    [HttpPost("google/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<AuthResponse>> GoogleRegister(GoogleRegister model)
    {
        return Ok(await _authenticationService.GoogleRegister(model));
    }

    // POST api/<AuthController>/refresh-token
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult> RefreshToken([FromBody] TokenModel model)
    {
        return Ok(await _authenticationService.RefreshToken(model));
    }
}
