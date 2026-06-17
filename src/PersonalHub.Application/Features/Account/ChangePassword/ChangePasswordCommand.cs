using MediatR;

namespace PersonalHub.Application.Features.Account.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public string UserId { get; set; } = string.Empty;

    public string CurrentPassword { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;
}