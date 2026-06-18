using MediatR;
using PersonalHub.Application.Features.SubFunds.Common;

namespace PersonalHub.Application.Features.SubFunds.GetSubFundById;

public record GetSubFundByIdQuery(Guid Id)
    : IRequest<SubFundDto?>;
