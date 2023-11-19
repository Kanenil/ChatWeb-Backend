using AutoMapper;
using ChatWeb.Application.DTOs.Messages;
using ChatWeb.Domain;

namespace ChatWeb.Application.Profiles;

public class MessagesProfile : Profile
{
    public MessagesProfile()
    {
        CreateMap<MessageEntity, MessageDTO>();
        CreateMap<CreateMessageDTO, MessageEntity>();
    }
}
