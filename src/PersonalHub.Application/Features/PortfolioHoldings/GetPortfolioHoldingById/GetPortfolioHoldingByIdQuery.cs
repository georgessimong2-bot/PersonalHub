using MediatR;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.PortfolioHoldings.GetPortfolioHoldingById;

public record GetPortfolioHoldingByIdQuery(Guid Id)
    : IRequest<PortfolioHoldingDto?>;
