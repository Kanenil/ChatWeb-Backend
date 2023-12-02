using Microsoft.AspNetCore.Http;

namespace ChatWeb.Application.DTOs.Chats;

public class CreateChatDTO
{
    public string Name { get; set; }
    public string? Image { get; set; }
}
