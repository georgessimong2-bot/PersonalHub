using MediatR;

namespace PersonalHub.Application.Features.Auth.Login;

public record LoginCommand(
    string Email,
    string Password)
    : IRequest<string>;