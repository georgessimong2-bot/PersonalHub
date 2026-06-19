using MediatR;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Users.AssignRole;

public class AssignRoleHandler : IRequestHandler<AssignRoleCommand, Unit>
{
    private readonly IIdentityService _identityService;

    public AssignRoleHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Unit> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        await _identityService.AssignRoleAsync(request.UserId, request.Role);
        return Unit.Value;
    }
}
