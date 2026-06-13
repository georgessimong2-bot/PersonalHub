using MediatR;

namespace PersonalHub.Application.Features.FundTypes.CreateFundType;

public record CreateFundTypeCommand(
    string Name,
    string? Description
) : IRequest<Guid>;