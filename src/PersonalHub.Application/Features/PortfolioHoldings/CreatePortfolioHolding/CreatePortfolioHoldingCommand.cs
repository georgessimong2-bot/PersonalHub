using MediatR;

namespace PersonalHub.Application.Features.PortfolioHoldings.CreatePortfolioHolding;

public class CreatePortfolioHoldingCommand : IRequest<Guid>
{
    public Guid PortfolioId { get; set; }

    public Guid InstrumentId { get; set; }

    public decimal Quantity { get; set; }

    public decimal? AverageCost { get; set; }
}
