using AutoMapper;
using ChatWeb.Application.DTOs.Chats;
using ChatWeb.Domain;

namespace ChatWeb.Application.Profiles;

public class ChatsProfile : Profile
{
    public ChatsProfile()
    {
        CreateMap<CreateChatDTO, ChatEntity>();
        CreateMap<ChatEntity, ChatDTO>()
            .ForMember(x => x.Users, x => x.MapFrom(x => x.ChatGroups.Select(x => x.User).Where(x => x.UserName != "ChatInfo")))
            .ForMember(x => x.LastMessage, x => x.MapFrom(x => x.Messages.OrderByDescending(x => x.DateCreated).FirstOrDefault()));  //;
    }
}
