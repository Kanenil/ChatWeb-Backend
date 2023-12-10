using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatWeb.Application.Hubs;

record HubUser(UserEntity User, string ConnectionId);

[Authorize]
public class ChatHub : Hub
{
    private readonly IUsersRepository _usersRepository;

    private static readonly List<HubUser> _users = new();

    public ChatHub(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var user = await _usersRepository.GetUserByUsernameAsync(Context.User.Identity.Name);

        _users.Add(new HubUser(user, Context.ConnectionId));

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

    public async Task LeaveFromChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        await Clients.Group(chatId.ToString()).SendAsync("UpdateChat", chatId);
    }

    public async Task InviteToChat(int chatId, int userId)
    {
        var user = await _usersRepository.GetAsync(userId);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());

        var hubUsers = _users.Where(x => x.User.Id == user.Id);

        foreach (var item in hubUsers)
        {
            await Groups.AddToGroupAsync(item.ConnectionId, chatId.ToString());
        }

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
            
            var connectionId = Context.ConnectionId;

            var hubUser = _users.Where(x => x.ConnectionId == connectionId).FirstOrDefault();

            if(hubUser != null)
            {
                _users.Remove(hubUser);
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
