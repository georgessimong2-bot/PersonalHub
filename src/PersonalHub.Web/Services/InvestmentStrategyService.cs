using PersonalHub.Application.Features.InvestmentStrategies.Common;
using PersonalHub.Application.Features.InvestmentStrategies.CreateInvestmentStrategy;
using PersonalHub.Application.Features.InvestmentStrategies.UpdateInvestmentStrategy;

namespace PersonalHub.Web.Services;

public class InvestmentStrategyService : BaseHttpService
{
    public InvestmentStrategyService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<InvestmentStrategyDto>> GetInvestmentStrategiesAsync()
    {
        return await GetAllAsync<InvestmentStrategyDto>("api/investment-strategies");
    }

    public async Task<InvestmentStrategyDto?> GetInvestmentStrategyByIdAsync(Guid id)
    {
        return await GetByIdAsync<InvestmentStrategyDto>($"api/investment-strategies/{id}");
    }

    public async Task<Guid> CreateInvestmentStrategyAsync(CreateInvestmentStrategyCommand command)
    {
        return await CreateAsync("api/investment-strategies", command);
    }

    public async Task UpdateInvestmentStrategyAsync(Guid id, UpdateInvestmentStrategyCommand command)
    {
        await UpdateAsync($"api/investment-strategies/{id}", command);
    }

    public async Task DeleteInvestmentStrategyAsync(Guid id)
    {
        await DeleteAsync($"api/investment-strategies/{id}");
    }
}
