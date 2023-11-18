using ChatWeb.Application.DTOs.Messages;
using ChatWeb.Application.DTOs.Users;

namespace ChatWeb.Application.DTOs.Chats;

public class ChatDTO
{
    public string Name { get; set; }
    public string? Image { get; set; }
    public IEnumerable<ChatUserDTO> Users { get; set; }
    public MessageDTO LastMessage { get; set; }
}
