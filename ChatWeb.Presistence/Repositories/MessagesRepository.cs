using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatWeb.Presistence.Repositories;

public class MessagesRepository : GenericRepository<MessageEntity>, IMessagesRepository
{
    public MessagesRepository(ChatDbContext dbContext) : base(dbContext)
    {}

    public async Task<IEnumerable<MessageEntity>> GetAllByChatIdAsync(int id)
    {
        return await _dbContext.Messages
            .Include(m => m.User)
            .Where(x => x.ChatId == id)
            .ToListAsync();
    }
}
