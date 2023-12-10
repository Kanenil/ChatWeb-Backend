using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.Exceptions;
using ChatWeb.Application.Features.Chats.Requests.Commands;
using ChatWeb.Application.Models.Responses;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Handlers.Commands;

public class ExitFromChatCommandHandler : IRequestHandler<ExitFromChatCommand, BaseCommandResponse>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMessagesRepository _messagesRepository;

    public ExitFromChatCommandHandler(IChatRepository chatRepository, IUsersRepository usersRepository, IMessagesRepository messagesRepository)
    {
        _chatRepository = chatRepository;
        _usersRepository = usersRepository;
        _messagesRepository = messagesRepository;
    }

    public async Task<BaseCommandResponse> Handle(ExitFromChatCommand request, CancellationToken cancellationToken)
    {
        BaseCommandResponse response = new();

        var chat = await _chatRepository.GetAsync(request.ChatId);
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);
        var system = await _usersRepository.GetUserByUsernameAsync("ChatInfo");

        if (!chat.ChatGroups.Select(x => x.UserId).Contains(user.Id))
        {
            throw new BadRequestException($"User is not in this chat!");
        }

        await _messagesRepository.AddAsync(new() { ChatId = chat.Id, UserId = system.Id, Content = $"{user.UserName} has left from the group" });
        _chatRepository.RemoveChatGroup(new() { ChatId = chat.Id, UserId = user.Id });
        await _chatRepository.SaveAsync();

        response.Success = true;
        response.Message = "Delete Successful";
        response.Id = -1;

        return response;
    }
}
