using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Domain;
using ChatWeb.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ChatWeb.Presistence.Repositories;

public class ChatRepository : GenericRepository<ChatEntity>, IChatRepository
{
    public ChatRepository(ChatDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ChatGroupEntity> AddChatGroupAsync(ChatGroupEntity entity)
    {
        await _dbContext.AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<ChatEntity>> GetAllByUserIdAsync(int id)
    {
        return await _dbContext.Chats
                        .Where(x=>x.ChatGroups.Select(x => x.UserId).Contains(id))
                        .ToListAsync();
    }

    public async Task<ChatEntity> GetByUserIdAsync(int id, int userId)
    {
        return await _dbContext.Chats
                        .Where(x => x.ChatGroups.Select(x => x.UserId).Contains(userId))
                        .Where(x => x.Id == id)
                        .FirstAsync();
    }
}
