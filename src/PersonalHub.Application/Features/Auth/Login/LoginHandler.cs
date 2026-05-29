using MediatR;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Auth.Login;

public class LoginHandler
    : IRequestHandler<LoginCommand, string>
{
    private readonly IIdentityService _identityService;

    public LoginHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.LoginAsync(
            request.Email,
            request.Password);
    }
}