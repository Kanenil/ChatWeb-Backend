using ChatWeb.Application.DTOs.Users;
using MediatR;

namespace ChatWeb.Application.Features.Users.Requests.Queries;

public record GetUsersListRequest(string SearchBy, string Username) : IRequest<List<UserDTO>>
{ }
