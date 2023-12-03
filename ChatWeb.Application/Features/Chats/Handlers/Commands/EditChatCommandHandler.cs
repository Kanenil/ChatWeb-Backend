using ChatWeb.Application.Contracts.Infrastructure;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.Features.Chats.Requests.Commands;
using ChatWeb.Application.Models.Responses;
using MediatR;
using System.Text.RegularExpressions;

namespace ChatWeb.Application.Features.Chats.Handlers.Commands;

public class EditChatCommandHandler : IRequestHandler<EditChatCommand, BaseCommandResponse>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IImageService _imageService;
    private readonly IMessagesRepository _messagesRepository;

    public EditChatCommandHandler(IChatRepository chatRepository, IUsersRepository usersRepository, IImageService imageService, IMessagesRepository messagesRepository)
    {
        _chatRepository = chatRepository;
        _usersRepository = usersRepository;
        _imageService = imageService;
        _messagesRepository = messagesRepository;
    }

    public async Task<BaseCommandResponse> Handle(EditChatCommand request, CancellationToken cancellationToken)
    {
        BaseCommandResponse response = new();

        var chat = await _chatRepository.GetAsync(request.ChatDTO.Id);
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);
        var system = await _usersRepository.GetUserByUsernameAsync("ChatInfo");

        chat.LastEditionAuthorId = user.Id;

        string message = "";

        if(chat.Name != request.ChatDTO.Name)
        {
            message = $"Group member {user.UserName} has changed a Group Name from \"{chat.Name}\" to \"{request.ChatDTO.Name}\"";
            chat.Name = request.ChatDTO.Name;
        }

        if (!string.IsNullOrEmpty(request.ChatDTO.Image) && request.ChatDTO.Image.Split(',').Length == 2)
        {
            message += string.IsNullOrEmpty(message) ? $"Group member {user.UserName} has changed a Group Image" : " and a Group Image";

            if(chat.Image != null)
            {
                _imageService.RemoveImage(chat.Image);
            }
            chat.Image = _imageService.SaveImageFromBase64(request.ChatDTO.Image);
        }

        if (string.IsNullOrEmpty(request.ChatDTO.Image) && chat.Image != null)
        {
            message += string.IsNullOrEmpty(message) ? $"Group member {user.UserName} has removed a Group Image" : " and removed a Group Image";
            
            _imageService.RemoveImage(chat.Image);
            chat.Image = null;
        }

        if(!string.IsNullOrEmpty(message))
        {
            await _messagesRepository.AddAsync(new() { ChatId = chat.Id, UserId = system.Id, Content = message });
        }
       
        await _chatRepository.UpdateAsync(chat);
        await _chatRepository.SaveAsync();

        response.Success = true;
        response.Message = "Edition Successful";
        response.Id = chat.Id;

        return response;
    }
}
