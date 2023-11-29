using AutoMapper;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Messages;
using ChatWeb.Application.Exceptions;
using ChatWeb.Application.Features.Messages.Requests.Queries;
using MediatR;

namespace ChatWeb.Application.Features.Messages.Handlers.Queries;

public class GetMessageByIdRequestHandler : IRequestHandler<GetMessageByIdRequest, MessageDTO>
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public GetMessageByIdRequestHandler(IMessagesRepository messagesRepository, IUsersRepository usersRepository, IMapper mapper)
    {
        _messagesRepository = messagesRepository;
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public async Task<MessageDTO> Handle(GetMessageByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);

        var message = await _messagesRepository.GetAsync(request.MessageId);

        if(!user.ChatGroups.Select(x=>x.ChatId).Contains(message.ChatId))
        {
            throw new BadRequestException($"User {request.Username} don't have chat with this message!");
        }

        return _mapper.Map<MessageDTO>(message);
    }
}
