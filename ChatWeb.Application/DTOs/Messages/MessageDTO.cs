using ChatWeb.Application.DTOs.Users;

namespace ChatWeb.Application.DTOs.Messages;

public class MessageDTO
{
    public string Content { get; set; }
    public ChatUserDTO User { get; set; }
}
