using MediatR;
using PersonalHub.Application.Features.Users.Common;

namespace PersonalHub.Application.Features.Users.GetUsers;

public record GetUsersCommand()
    : IRequest<List<UserDto>>;