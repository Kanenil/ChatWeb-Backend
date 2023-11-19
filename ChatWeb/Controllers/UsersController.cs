using ChatWeb.API.Middleware;
using ChatWeb.Application.DTOs.Users;
using ChatWeb.Application.Features.Users.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatWeb.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET api/<UsersController>/?{searchBy}
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<UserDTO>> Get([FromQuery] string searchBy)
    {
        string username = User.FindFirstValue(ClaimTypes.Name);
        var users = await _mediator.Send(new GetUsersListRequest(searchBy, username));
        return Ok(users);
    }

}
