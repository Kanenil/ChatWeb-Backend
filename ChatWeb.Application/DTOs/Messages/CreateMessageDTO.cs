using Microsoft.AspNetCore.Http;

namespace ChatWeb.Application.DTOs.Messages;

public record CreateMessageDTO(string? Content, IFormFile? File, int ChatId)
{}
