using MediatR;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Auth.Register;

public class RegisterHandler
    : IRequestHandler<RegisterCommand, string>
{
    private readonly IIdentityService _identityService;

    public RegisterHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.RegisterAsync(
            request.Email,
            request.Password);
    }
}