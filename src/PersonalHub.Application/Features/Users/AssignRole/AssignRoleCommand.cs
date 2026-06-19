using MediatR;

namespace PersonalHub.Application.Features.Users.AssignRole;

public class AssignRoleCommand : IRequest<Unit>
{
    public string UserId { get; set; } = null!;
    public string Role { get; set; } = null!;
}
