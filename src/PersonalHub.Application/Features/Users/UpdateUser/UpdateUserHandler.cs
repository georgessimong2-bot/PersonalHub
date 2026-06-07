using MediatR;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Users.UpdateUser;

public class UpdateUserHandler
    : IRequestHandler<UpdateUserCommand>
{
    private readonly IIdentityService _identityService;

    public UpdateUserHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        await _identityService.UpdateUserAsync(
            request.Id,
            request.Email,
            request.Role);
    }
}