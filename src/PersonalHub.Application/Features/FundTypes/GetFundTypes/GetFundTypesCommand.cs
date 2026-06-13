using MediatR;
using PersonalHub.Application.Features.FundTypes.Common;

namespace PersonalHub.Application.Features.FundTypes.GetFundTypes;

public record GetFundTypesCommand()
    : IRequest<List<FundTypeDto>>;