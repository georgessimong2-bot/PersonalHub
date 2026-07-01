using MediatR;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Application.Features.Portfolios.GetPortfolioById;

public record GetPortfolioByIdQuery(Guid Id)
    : IRequest<PortfolioDto?>;
