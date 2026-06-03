using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Users.Common;

namespace PersonalHub.Application.Features.Users.GetUsers;

public class GetUsersHandler
    : IRequestHandler<GetUsersCommand, List<UserDto>>
{
    private readonly IIdentityService _identityService;

    public GetUsersHandler(
        IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<List<UserDto>> Handle(
        GetUsersCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.GetUsersAsync();
    }
}