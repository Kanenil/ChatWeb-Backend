using AutoMapper;
using ChatWeb.Application.Contracts.Identity;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Users;
using ChatWeb.Application.Exceptions;

namespace ChatWeb.Infrastructure.Identity
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public AccountService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task Logout(string username)
        {
            var user = await _usersRepository.GetUserByUsernameAsync(username);

            if (user == null)
                throw new BadRequestException($"User with '{username}' not exists.");

            user.RefreshToken = null;
            await _usersRepository.SaveAsync();
        }

        public async Task<UserDTO> Profile(string username)
        {
            var user = await _usersRepository.GetUserByUsernameAsync(username);

            if (user == null)
                throw new NotFoundException("User", username);

            return _mapper.Map<UserDTO>(user);
        }
    }
}
