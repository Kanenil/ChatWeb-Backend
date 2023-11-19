using AutoMapper;
using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Application.DTOs.Users;
using ChatWeb.Application.Features.Users.Requests.Queries;
using MediatR;

namespace ChatWeb.Application.Features.Users.Handlers.Queries;

public class GetUsersListRequestHandler : IRequestHandler<GetUsersListRequest, List<UserDTO>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public GetUsersListRequestHandler(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public async Task<List<UserDTO>> Handle(GetUsersListRequest request, CancellationToken cancellationToken)
    {
        var users = await _usersRepository.GetUsersByUsernameSearchAsync(request.SearchBy, request.Username);

        return _mapper.Map<List<UserDTO>>(users);
    }
}
