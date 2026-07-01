using MediatR;

namespace PersonalHub.Application.Features.Portfolios.CreatePortfolio;

public class CreatePortfolioCommand : IRequest<Guid>
{
    public Guid ShareClassId { get; set; }

    public DateTime ValuationDate { get; set; }

    public bool IsActive { get; set; } = true;
}
