using ChatWeb.Domain.Identity;

namespace ChatWeb.Application.Contracts.Persistence;

public interface IUsersRepository : IGenericRepository<UserEntity>
{
    public Task<UserEntity> GetUserByUsernameAsync(string username);
}
