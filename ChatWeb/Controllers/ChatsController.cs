using ChatWeb.API.Middleware;
using ChatWeb.Application.DTOs.Chats;
using ChatWeb.Application.Features.Chats.Requests.Commands;
using ChatWeb.Application.Features.Chats.Requests.Queries;
using ChatWeb.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatWeb.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<ChatsController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ChatDTO>>> Get()
    {
        string username = User.FindFirstValue(ClaimTypes.Name);
        var chats = await _mediator.Send(new GetChatsListRequest(username));
        return Ok(chats);
    }

    // GET api/<ChatsController>/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<ChatDTO>> Get(int id)
    {
        string username = User.FindFirstValue(ClaimTypes.Name);
        var chats = await _mediator.Send(new GetChatByIdRequest(id, username));
        return Ok(chats);
    }

    // POST api/<ChatsController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateChatDTO chat)
    {
        string username = User.FindFirstValue(ClaimTypes.Name);
        var command = new CreateChatCommand(chat, username);
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
