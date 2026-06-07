using MediatR;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Users.DeleteUser;

public class DeleteUserHandler
    : IRequestHandler<DeleteUserCommand>
{
    private readonly IIdentityService _identityService;

    public DeleteUserHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        await _identityService.DeleteUserAsync(
            request.Id);
    }
}