using MediatR;

namespace PersonalHub.Application.Features.PortfolioHoldings.UpdatePortfolioHolding;

public record UpdatePortfolioHoldingCommand(
    Guid Id,
    Guid InstrumentId,
    decimal Quantity,
    decimal? AverageCost)
    : IRequest;
