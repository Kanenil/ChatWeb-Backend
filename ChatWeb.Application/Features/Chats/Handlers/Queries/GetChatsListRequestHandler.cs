using AutoMapper;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Chats;
using ChatWeb.Application.Features.Chats.Requests.Queries;
using MediatR;

namespace ChatWeb.Application.Features.Chats.Handlers.Queries;

public class GetChatsListRequestHandler : IRequestHandler<GetChatsListRequest, List<ChatDTO>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public GetChatsListRequestHandler(IChatRepository chatRepository, IUsersRepository usersRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public async Task<List<ChatDTO>> Handle(GetChatsListRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);
        var chats = await _chatRepository.GetAllByUserIdAsync(user.Id);
        return _mapper.Map<List<ChatDTO>>(chats);
    }
}
