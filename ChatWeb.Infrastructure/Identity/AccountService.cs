using AutoMapper;
using ChatWeb.Application.Contracts.Identity;
using ChatWeb.Application.Contracts.Infrastructure;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Users;
using ChatWeb.Application.Exceptions;

namespace ChatWeb.Infrastructure.Identity
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUploadService _uploadService;
        private readonly IMapper _mapper;

        public AccountService(IUsersRepository usersRepository, IMapper mapper, IUploadService uploadService)
        {
            _usersRepository = usersRepository;
            _uploadService = uploadService;
            _mapper = mapper;
        }

        public async Task<UserDTO> EditProfile(UserDTO userDTO, string username)
        {
            var user = await _usersRepository.GetUserByUsernameAsync(username);

            if(!string.IsNullOrEmpty(userDTO.Image) && userDTO.Image.Split(',').Length == 2)
            {
                if (user.Image != null)
                {
                    _uploadService.RemoveImage(user.Image);
                }
                user.Image = _uploadService.SaveImageFromBase64(userDTO.Image);
            }

            if (string.IsNullOrEmpty(userDTO.Image) && user.Image != null)
            {
                _uploadService.RemoveImage(user.Image);
                user.Image = null;
            }

            await _usersRepository.SaveAsync();

            return _mapper.Map<UserDTO>(user);
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
