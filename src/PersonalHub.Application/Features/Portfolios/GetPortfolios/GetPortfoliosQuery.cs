using MediatR;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.Portfolios.GetPortfolios;

public record GetPortfoliosQuery(Guid? ShareClassId = null)
    : IRequest<List<PortfolioDto>>;
