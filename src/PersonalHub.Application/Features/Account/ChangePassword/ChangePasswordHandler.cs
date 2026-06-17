using MediatR;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Account.ChangePassword;

public class ChangePasswordHandler
    : IRequestHandler<ChangePasswordCommand>
{
    private readonly IIdentityService _identityService;

    public ChangePasswordHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        await _identityService.ChangePasswordAsync(
            request.UserId,
            request.CurrentPassword,
            request.NewPassword);
    }
}