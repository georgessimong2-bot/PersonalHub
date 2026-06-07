using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Users.Common;

namespace PersonalHub.Application.Features.Users.GetUserById;

public class GetUserByIdHandler
    : IRequestHandler<GetUserByIdCommand, UserDto?>
{
    private readonly IIdentityService _identityService;

    public GetUserByIdHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<UserDto?> Handle(
        GetUserByIdCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.GetUserByIdAsync(
            request.Id);
    }
}