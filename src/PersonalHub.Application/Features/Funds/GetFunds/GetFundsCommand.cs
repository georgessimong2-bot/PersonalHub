using MediatR;
using PersonalHub.Application.Features.Funds.Common;

namespace PersonalHub.Application.Features.Funds.GetFunds;

public record GetFundsCommand()
    : IRequest<List<FundDto>>;