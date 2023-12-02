using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.Exceptions;
using ChatWeb.Application.Features.Chats.Requests.Commands;
using ChatWeb.Application.Hubs;
using ChatWeb.Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatWeb.Application.Features.Chats.Handlers.Commands;

public class AddUserToChatCommandHandler : IRequestHandler<AddUserToChatCommand, BaseCommandResponse>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUsersRepository _usersRepository;

    public AddUserToChatCommandHandler(IChatRepository chatRepository, IUsersRepository usersRepository)
    {
        _chatRepository = chatRepository;
        _usersRepository = usersRepository;
    }

    public async Task<BaseCommandResponse> Handle(AddUserToChatCommand request, CancellationToken cancellationToken)
    {
        BaseCommandResponse response = new();

        var chat = await _chatRepository.GetAsync(request.ChatId);

        if (chat.ChatGroups.Select(x => x.UserId).Contains(request.UserId))
        {
            throw new BadRequestException($"User is already in this chat!");
        }

        if(!await _usersRepository.ExistsAsync(request.UserId))
        {
            throw new NotFoundException("UserId", request.UserId);
        }

        await _chatRepository.AddChatGroupAsync(new() { ChatId = chat.Id, UserId = request.UserId });
        await _chatRepository.SaveAsync();

        var user = await _usersRepository.GetAsync(request.UserId);

        response.Success = true;
        response.Message = "Creation Successful";
        response.Id = -1;

        return response;
    }
}
