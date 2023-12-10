using ChatWeb.Application.Models.Responses;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Requests.Commands;

public record ExitFromChatCommand(int ChatId, string Username) : IRequest<BaseCommandResponse>
{ }
