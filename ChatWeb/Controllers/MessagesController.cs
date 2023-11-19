using ChatWeb.API.Middleware;
using ChatWeb.Application.DTOs.Chats;
using ChatWeb.Application.DTOs.Messages;
using ChatWeb.Application.Features.Messages.Requests.Commands;
using ChatWeb.Application.Features.Messages.Requests.Queries;
using ChatWeb.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatWeb.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET api/<MessagesController>/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<ChatDTO>> Get(int id)
    {
        string username = User.FindFirstValue(ClaimTypes.Name);
        var messages = await _mediator.Send(new GetMessagesListRequest(username, id));
        return Ok(messages);
    }

    // POST api/<MessagesController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDeatils))]
    public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateMessageDTO message)
    {
        string username = User.FindFirstValue(ClaimTypes.Name);
        var command = new CreateMessageCommand(message, username);
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
