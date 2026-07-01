using MediatR;

namespace PersonalHub.Application.Features.PortfolioHoldings.DeletePortfolioHolding;

public record DeletePortfolioHoldingCommand(Guid Id) : IRequest;
