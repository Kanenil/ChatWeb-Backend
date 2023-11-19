using AutoMapper;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.Exceptions;
using ChatWeb.Application.Features.Messages.Requests.Commands;
using ChatWeb.Application.Models.Responses;
using ChatWeb.Domain;
using MediatR;

namespace ChatWeb.Application.Features.Messages.Handlers.Commands;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, BaseCommandResponse>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IMapper _mapper;

    public CreateMessageCommandHandler(IUsersRepository usersRepository, IMessagesRepository messagesRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _messagesRepository = messagesRepository;
        _mapper = mapper;
    }

    public async Task<BaseCommandResponse> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        BaseCommandResponse response = new();

        var message = _mapper.Map<MessageEntity>(request.MessageDTO);
        var user = await _usersRepository.GetUserByUsernameAsync(request.Username);

        if(!user.ChatGroups.Select(x => x.ChatId).Contains(request.MessageDTO.ChatId))
        {
            throw new BadRequestException($"User {request.Username} don't have this chat!");
        }

        message.UserId = user.Id;

        await _messagesRepository.AddAsync(message);
        await _messagesRepository.SaveAsync();

        response.Success = true;
        response.Message = "Creation Successful";
        response.Id = message.Id;

        return response;
    }
}
