using MediatR;

namespace PersonalHub.Application.Features.Users.DeleteUser;

public record DeleteUserCommand(
    string Id)
    : IRequest;