using MediatR;
using PersonalHub.Application.Features.Users.Common;

namespace PersonalHub.Application.Features.Users.GetUserById;

public record GetUserByIdCommand(
    string Id)
    : IRequest<UserDto>;