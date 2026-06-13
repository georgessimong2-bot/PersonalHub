using MediatR;
using PersonalHub.Application.Features.Funds.Common;

namespace PersonalHub.Application.Features.Funds.GetFundById;

public record GetFundByIdCommand(Guid Id)
    : IRequest<FundDto>;