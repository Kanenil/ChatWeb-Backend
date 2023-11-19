using ChatWeb.Application.DTOs.Users;

namespace ChatWeb.Application.Contracts.Identity;

public interface IAccountService
{
    Task<UserDTO> Profile(string username);
    Task Logout(string username);
}
