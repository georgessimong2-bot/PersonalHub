using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Account.UpdateProfile;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand>
{
    private readonly IIdentityService _identity;

    public UpdateProfileHandler(IIdentityService identity)
    {
        _identity = identity;
    }

    public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        await _identity.UpdateProfileAsync(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Address,
            request.PhoneNumber);
    }
}