using MediatR;

namespace PersonalHub.Application.Features.Portfolios.UpdatePortfolio;

public record UpdatePortfolioCommand(
    Guid Id,
    Guid ShareClassId,
    DateTime ValuationDate,
    bool IsActive)
    : IRequest;
