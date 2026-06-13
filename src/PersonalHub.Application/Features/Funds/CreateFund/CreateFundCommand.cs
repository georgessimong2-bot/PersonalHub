using MediatR;

namespace PersonalHub.Application.Features.Funds.CreateFund;

public record CreateFundCommand(
    string Name,
    Guid FundTypeId
) : IRequest<Guid>;