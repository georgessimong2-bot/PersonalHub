using MediatR;

namespace PersonalHub.Application.Features.Users.CreateUser;

public class CreateUserCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "USER";
}