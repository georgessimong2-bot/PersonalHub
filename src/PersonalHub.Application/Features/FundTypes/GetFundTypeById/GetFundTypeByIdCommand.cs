using MediatR;
using PersonalHub.Application.Features.FundTypes.Common;

namespace PersonalHub.Application.Features.FundTypes.GetFundTypeById;

public record GetFundTypeByIdCommand(Guid Id)
    : IRequest<FundTypeDto>;