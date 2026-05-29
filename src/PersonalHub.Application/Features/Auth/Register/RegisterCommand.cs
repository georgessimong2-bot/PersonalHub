using MediatR;

namespace PersonalHub.Application.Features.Auth.Register;

public record RegisterCommand(
    string Email,
    string Password)
    : IRequest<string>;