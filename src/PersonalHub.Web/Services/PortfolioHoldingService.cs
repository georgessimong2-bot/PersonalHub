using PersonalHub.Application.Features.Portfolios.Common;
using PersonalHub.Application.Features.PortfolioHoldings.CreatePortfolioHolding;
using PersonalHub.Application.Features.PortfolioHoldings.UpdatePortfolioHolding;

namespace PersonalHub.Web.Services;

public class PortfolioHoldingService : BaseHttpService
{
    public PortfolioHoldingService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<PortfolioHoldingDto>> GetPortfolioHoldingsAsync(Guid portfolioId)
    {
        return await GetAllAsync<PortfolioHoldingDto>($"api/portfolio-holdings?portfolioId={portfolioId}");
    }

    public async Task<PortfolioHoldingDto?> GetPortfolioHoldingByIdAsync(Guid id)
    {
        return await GetByIdAsync<PortfolioHoldingDto>($"api/portfolio-holdings/{id}");
    }

    public async Task<Guid> CreatePortfolioHoldingAsync(CreatePortfolioHoldingCommand command)
    {
        return await CreateAsync("api/portfolio-holdings", command);
    }

    public async Task UpdatePortfolioHoldingAsync(Guid id, UpdatePortfolioHoldingCommand command)
    {
        await UpdateAsync($"api/portfolio-holdings/{id}", command);
    }

    public async Task DeletePortfolioHoldingAsync(Guid id)
    {
        await DeleteAsync($"api/portfolio-holdings/{id}");
    }
}
