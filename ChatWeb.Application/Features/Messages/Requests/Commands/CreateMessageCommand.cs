using ChatWeb.Application.DTOs.Messages;
using ChatWeb.Application.Models.Responses;
using MediatR;

namespace ChatWeb.Application.Features.Messages.Requests.Commands;

public record CreateMessageCommand(CreateMessageDTO MessageDTO, string Username) : IRequest<BaseCommandResponse>
{ }
