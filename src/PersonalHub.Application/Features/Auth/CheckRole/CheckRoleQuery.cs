using MediatR;
using PersonalHub.Application.Common.Interfaces;
using System.Security.Claims;

namespace PersonalHub.Application.Features.Auth.CheckRole;

public class CheckRoleQuery : IRequest<bool>
{
    public string RequiredRole { get; set; } = null!;
}

public class CheckRoleHandler : IRequestHandler<CheckRoleQuery, bool>
{
    private readonly ICurrentUserService _currentUserService;

    public CheckRoleHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public Task<bool> Handle(CheckRoleQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
            return Task.FromResult(false);

        // Cette vérification doit être complétée par l'API avec [Authorize(Roles = "ADMIN")]
        // Le endpoint lui-même utilisera l'authorization attribute pour vérifier le rôle
        return Task.FromResult(true);
    }
}
