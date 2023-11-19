using AutoMapper;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Messages;
using ChatWeb.Application.Exceptions;
using ChatWeb.Application.Features.Messages.Requests.Queries;
using MediatR;

namespace ChatWeb.Application.Features.Messages.Handlers.Queries;

public class GetMessagesListRequestHandler : IRequestHandler<GetMessagesListRequest, List<MessageDTO>>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public GetMessagesListRequestHandler(IMessagesRepository messagesRepository, IUsersRepository usersRepository, IMapper mapper)
    {
        _messagesRepository = messagesRepository;
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public async Task<List<MessageDTO>> Handle(GetMessagesListRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);

        if (!user.ChatGroups.Select(x => x.ChatId).Contains(request.ChatId))
        {
            throw new BadRequestException($"User {request.Username} don't have this chat!");
        }
        
        var messages = await _messagesRepository.GetAllByChatIdAsync(user.Id);
        return _mapper.Map<List<MessageDTO>>(messages);
    }
}
