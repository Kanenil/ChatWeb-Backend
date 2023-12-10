using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatWeb.Presistence.Repositories;

public class ChatRepository : GenericRepository<ChatEntity>, IChatRepository
{
    public ChatRepository(ChatDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<ChatEntity> GetAsync(int id)
    {
        return await _dbContext.Chats
            .Include(x => x.ChatGroups)
            .Where(x => x.Id ==  id)
            .FirstAsync();
    }

    public async Task<ChatGroupEntity> AddChatGroupAsync(ChatGroupEntity entity)
    {
        await _dbContext.AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<ChatEntity>> GetAllByUserIdAsync(int id)
    {
        return await _dbContext.Chats
            .Include(x => x.ChatGroups)
                .ThenInclude(x => x.User)
            .Include(x => x.Messages)
            .Where(x => x.ChatGroups.Select(x => x.UserId).Contains(id))
            .ToListAsync();
    }

    public async Task<ChatEntity?> GetByUserIdAsync(int id, int userId)
    {
        return await _dbContext.Chats
            .Include(x => x.ChatGroups)
                .ThenInclude(x => x.User)
            .Include(x => x.Messages)
            .Where(x => x.ChatGroups.Select(x => x.UserId).Contains(userId))
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async void RemoveChatGroup(ChatGroupEntity chatGroup)
    {
        var chatGroupToDelete = await _dbContext.ChatGroups.FindAsync(chatGroup.UserId, chatGroup.ChatId);

        if (chatGroupToDelete != null)
        {
            _dbContext.ChatGroups.Remove(chatGroupToDelete);
        }

        _dbContext.Remove(chatGroup);
    }
}
