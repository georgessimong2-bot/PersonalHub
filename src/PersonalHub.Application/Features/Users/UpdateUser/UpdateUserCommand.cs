using MediatR;

namespace PersonalHub.Application.Features.Users.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "User";
}