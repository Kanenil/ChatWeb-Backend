using ChatWeb.Domain;

namespace ChatWeb.Application.Contracts.Persistence;

public interface IChatRepository : IGenericRepository<ChatEntity>
{
    Task<IEnumerable<ChatEntity>> GetAllByUserIdAsync(int id);
    Task<ChatEntity> GetByUserIdAsync(int id, int userId);
    Task<ChatGroupEntity> AddChatGroupAsync(ChatGroupEntity chatGroup);
    void RemoveChatGroup(ChatGroupEntity chatGroup);
}
