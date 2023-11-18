using AutoMapper;
using ChatWeb.Application.DTOs.Users;
using ChatWeb.Application.Models.Requests;
using ChatWeb.Domain.Identity;

namespace ChatWeb.Application.Profiles;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<RegistrationRequest, UserEntity>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));
        CreateMap<UserEntity, ChatUserDTO>();
    }
}
