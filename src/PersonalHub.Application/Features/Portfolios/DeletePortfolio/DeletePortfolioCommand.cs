using MediatR;

namespace PersonalHub.Application.Features.Portfolios.DeletePortfolio;

public record DeletePortfolioCommand(Guid Id) : IRequest;
