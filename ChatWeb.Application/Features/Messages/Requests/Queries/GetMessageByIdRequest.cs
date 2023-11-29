using ChatWeb.Application.DTOs.Messages;
using MediatR;

namespace ChatWeb.Application.Features.Messages.Requests.Queries;

public record GetMessageByIdRequest(string Username, int MessageId) : IRequest<MessageDTO>
{}
