using AutoMapper;
using ChatWeb.Application.DTOs.Messages;
using ChatWeb.Domain;

namespace ChatWeb.Application.Profiles;

public class MessagesProfile : Profile
{
    public MessagesProfile()
    {
        CreateMap<MessageEntity, MessageDTO>();
        CreateMap<CreateMessageDTO, MessageEntity>()
            .ForMember(x => x.Content, x => x.MapFrom(x => x.Content ?? ""))
            .ForMember(x => x.FileName, x => x.MapFrom(x => ""));
    }
}
