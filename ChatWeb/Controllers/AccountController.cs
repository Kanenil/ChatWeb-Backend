using ChatWeb.Application.Contracts.Identity;
using ChatWeb.Application.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatWeb.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<UserDTO>> Profile()
    {
        string name = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _accountService.Profile(name));
    }

    [HttpPost("edit")]
    public async Task<ActionResult<UserDTO>> Edit(UserDTO user)
    {
        string name = User.FindFirstValue(ClaimTypes.Name);
        return Ok(await _accountService.EditProfile(user, name));
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        string name = User.FindFirstValue(ClaimTypes.Name);
        await _accountService.Logout(name);
        return NoContent();
    }
}
