using ChatWeb.Application.DTOs.Users;

namespace ChatWeb.Application.DTOs.Messages;

public record MessageDTO(int Id, string Content, string FileName, DateTime DateCreated, ChatUserDTO User)
{}
