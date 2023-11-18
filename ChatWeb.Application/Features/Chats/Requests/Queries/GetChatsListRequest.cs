using ChatWeb.Application.DTOs.Chats;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Requests.Queries;

public record GetChatsListRequest(string Username) : IRequest<List<ChatDTO>>
{}
