using AutoMapper;
using ChatWeb.Application.DTOs.Users;
using ChatWeb.Application.Models.Requests;
using ChatWeb.Domain.Identity;

namespace ChatWeb.Application.Profiles;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<RegistrationRequest, UserEntity>();
        CreateMap<UserEntity, ChatUserDTO>();
        CreateMap<UserEntity, UserDTO>();
    }
}
