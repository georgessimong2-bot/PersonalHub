using MediatR;
using PersonalHub.Application.Features.SubFunds.Common;

namespace PersonalHub.Application.Features.SubFunds.GetSubFunds;

public record GetSubFundsQuery()
    : IRequest<List<SubFundDto>>;
