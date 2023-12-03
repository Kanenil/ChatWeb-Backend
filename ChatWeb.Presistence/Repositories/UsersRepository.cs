using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatWeb.Presistence.Repositories;

public class UsersRepository : GenericRepository<UserEntity>, IUsersRepository
{
    public UsersRepository(ChatDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IReadOnlyList<UserEntity>> GetAllAsync()
    {
        return await _dbContext.Users
            .Include(x => x.Messages)
                .ThenInclude(x => x.Chat)
            .Include(x => x.ChatGroups)
                .ThenInclude(x => x.Chat)
            .ToListAsync();
    }

    public override async Task<UserEntity> GetAsync(int id)
    {
        return await _dbContext.Users
            .Include(x => x.Messages)
                .ThenInclude(x => x.Chat)
            .Include(x => x.ChatGroups)
                .ThenInclude(x => x.Chat)
            .FirstAsync(x => x.Id == id);
    }

    public async Task<UserEntity> GetUserByUsernameAsync(string username)
    {
        return await _dbContext.Users
            .Include(x => x.Messages)
                .ThenInclude(x => x.Chat)
            .Include(x => x.ChatGroups)
                .ThenInclude(x => x.Chat)
            .FirstAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<UserEntity>> GetUsersByUsernameSearchAsync(string searchBy, string username)
    {
        return await _dbContext.Users
            .Include(x => x.Messages)
                .ThenInclude(x => x.Chat)
            .Include(x => x.ChatGroups)
                .ThenInclude(x => x.Chat)
            .Where(x => x.UserName.ToLower().Contains(searchBy.ToLower()))
            .Where(x => x.UserName != username)
            .Where(x => x.UserName != "ChatInfo")
            .ToListAsync();
    }
}
