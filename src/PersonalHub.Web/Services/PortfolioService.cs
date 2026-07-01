using PersonalHub.Application.Features.Portfolios.Common;
using PersonalHub.Application.Features.Portfolios.CreatePortfolio;
using PersonalHub.Application.Features.Portfolios.UpdatePortfolio;

namespace PersonalHub.Web.Services;

public class PortfolioService : BaseHttpService
{
    public PortfolioService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<PortfolioDto>> GetPortfoliosAsync(Guid? shareClassId = null)
    {
        var endpoint = shareClassId.HasValue
            ? $"api/portfolios?shareClassId={shareClassId.Value}"
            : "api/portfolios";

        return await GetAllAsync<PortfolioDto>(endpoint);
    }

    public async Task<PortfolioDto?> GetPortfolioByIdAsync(Guid id)
    {
        return await GetByIdAsync<PortfolioDto>($"api/portfolios/{id}");
    }

    public async Task<Guid> CreatePortfolioAsync(CreatePortfolioCommand command)
    {
        return await CreateAsync("api/portfolios", command);
    }

    public async Task UpdatePortfolioAsync(Guid id, UpdatePortfolioCommand command)
    {
        await UpdateAsync($"api/portfolios/{id}", command);
    }

    public async Task DeletePortfolioAsync(Guid id)
    {
        await DeleteAsync($"api/portfolios/{id}");
    }
}
