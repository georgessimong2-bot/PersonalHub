using MediatR;

namespace PersonalHub.Application.Features.FundTypes.UpdateFundType;

public record UpdateFundTypeCommand(
    Guid Id,
    string Name,
    string? Description
) : IRequest;