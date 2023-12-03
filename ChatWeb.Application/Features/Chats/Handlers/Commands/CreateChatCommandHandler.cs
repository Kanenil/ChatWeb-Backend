using AutoMapper;
using ChatWeb.Application.Contracts.Infrastructure;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.Features.Chats.Requests.Commands;
using ChatWeb.Application.Models.Responses;
using ChatWeb.Domain;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Handlers.Commands;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, BaseCommandResponse>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public CreateChatCommandHandler(IChatRepository chatRepository, IUsersRepository usersRepository, IMapper mapper, IImageService imageService)
    {
        _chatRepository = chatRepository;
        _usersRepository = usersRepository;
        _imageService = imageService;
        _mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        BaseCommandResponse response = new();

        var chat = _mapper.Map<ChatEntity>(request.ChatDTO);
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);
        var system = await _usersRepository.GetUserByUsernameAsync("ChatInfo");
        
        chat.CreateAuthorId = user.Id;
        chat.LastEditionAuthorId = user.Id;

        if (!string.IsNullOrEmpty(request.ChatDTO.Image))
        {
            chat.Image = _imageService.SaveImageFromBase64(request.ChatDTO.Image);
        }

        await _chatRepository.AddAsync(chat);
        await _chatRepository.SaveAsync();

        await _chatRepository.AddChatGroupAsync(new() { ChatId = chat.Id, UserId = user.Id });
        await _chatRepository.AddChatGroupAsync(new() { ChatId = chat.Id, UserId = system.Id });
        await _chatRepository.SaveAsync();

        response.Success = true;
        response.Message = "Creation Successful";
        response.Id = chat.Id;

        return response;
    }
}
