using MediatR;

namespace PersonalHub.Application.Features.Funds.UpdateFund;

public record UpdateFundCommand(
    Guid Id,
    string Name,
    Guid FundTypeId
) : IRequest;