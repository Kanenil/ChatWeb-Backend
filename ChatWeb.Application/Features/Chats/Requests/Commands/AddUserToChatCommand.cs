using ChatWeb.Application.Models.Responses;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Requests.Commands;

public record AddUserToChatCommand(int ChatId, int UserId) : IRequest<BaseCommandResponse>
{ }

