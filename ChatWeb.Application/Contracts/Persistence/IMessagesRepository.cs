using ChatWeb.Domain;

namespace ChatWeb.Application.Contracts.Persistence;

public interface IMessagesRepository : IGenericRepository<MessageEntity>
{
    Task<IEnumerable<MessageEntity>> GetAllByChatIdAsync(int id);
}
