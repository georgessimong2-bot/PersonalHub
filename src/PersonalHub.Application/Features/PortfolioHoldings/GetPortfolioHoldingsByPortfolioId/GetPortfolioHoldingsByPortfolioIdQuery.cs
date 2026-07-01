using MediatR;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.PortfolioHoldings.GetPortfolioHoldingsByPortfolioId;

public record GetPortfolioHoldingsByPortfolioIdQuery(Guid PortfolioId)
    : IRequest<List<PortfolioHoldingDto>>;
