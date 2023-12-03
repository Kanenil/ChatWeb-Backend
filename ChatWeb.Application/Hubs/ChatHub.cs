using AutoMapper;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Users;
using ChatWeb.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatWeb.Application.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUsersRepository _usersRepository;


    public ChatHub(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var user = await _usersRepository.GetUserByUsernameAsync(Context.User.Identity.Name);

        foreach (var group in user.ChatGroups)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group.ChatId.ToString());
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{user.UserName}");

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(int chatId, int messageId)
    {
        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", chatId, messageId);
    }

    public async Task InviteToChat(int chatId, int userId)
    {
        var user = await _usersRepository.GetAsync(userId);

        await Clients.Group($"user:{user.UserName}").SendAsync("AddedToChat", chatId);
    }

    public async Task UpdateChat(int chatId)
    {
        await Clients.Group(chatId.ToString()).SendAsync("UpdateChat", chatId);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var user = await _usersRepository.GetUserByUsernameAsync(Context.User.Identity.Name);

            foreach (var group in user.ChatGroups)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group.ChatId.ToString());
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{user.UserName}");

            await base.OnDisconnectedAsync(exception);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
        }


        await base.OnDisconnectedAsync(exception);
    }
}
