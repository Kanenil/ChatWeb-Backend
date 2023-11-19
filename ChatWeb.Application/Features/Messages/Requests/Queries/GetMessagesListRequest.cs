using ChatWeb.Application.DTOs.Messages;
using MediatR;

namespace ChatWeb.Application.Features.Messages.Requests.Queries;

public record GetMessagesListRequest(string Username, int ChatId) : IRequest<List<MessageDTO>>
{}
