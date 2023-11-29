using AutoMapper;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Chats;
using ChatWeb.Application.Features.Chats.Requests.Queries;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Handlers.Queries;

public class GetChatByIdRequestHandler : IRequestHandler<GetChatByIdRequest, ChatDTO>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IMapper _mapper;

    public GetChatByIdRequestHandler(IChatRepository chatRepository, IUsersRepository usersRepository, IMapper mapper, IMessagesRepository messagesRepository)
    {
        _chatRepository = chatRepository;
        _usersRepository = usersRepository;
        _mapper = mapper;
        _messagesRepository = messagesRepository;
    }

    public async Task<ChatDTO> Handle(GetChatByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);
        var chat = await _chatRepository.GetByUserIdAsync(request.ChatId, user.Id);
        return _mapper.Map<ChatDTO>(chat);
    }
}
