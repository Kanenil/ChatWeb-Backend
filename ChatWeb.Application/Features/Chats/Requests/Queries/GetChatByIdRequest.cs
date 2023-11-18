using ChatWeb.Application.DTOs.Chats;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Requests.Queries;

public record GetChatByIdRequest(int ChatId, string Username) : IRequest<ChatDTO>
{
}
