using ChatWeb.Application.DTOs.Chats;
using ChatWeb.Application.Models.Responses;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Requests.Commands;

public record CreateChatCommand(CreateChatDTO ChatDTO, string Username) : IRequest<BaseCommandResponse>
{ }
