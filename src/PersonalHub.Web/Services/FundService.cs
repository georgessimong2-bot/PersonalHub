using PersonalHub.Application.Features.Funds.Common;
using PersonalHub.Application.Features.Funds.CreateFund;
using PersonalHub.Application.Features.Funds.UpdateFund;

namespace PersonalHub.Web.Services;

public class FundService : BaseHttpService
{
    public FundService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<FundDto>> GetFundsAsync()
    {
        return await GetAllAsync<FundDto>("api/funds");
    }

    public async Task<FundDto?> GetFundByIdAsync(Guid id)
    {
        return await GetByIdAsync<FundDto>($"api/funds/{id}");
    }

    public async Task CreateFundAsync(CreateFundCommand command)
    {
        await CreateAsync("api/funds", command);
    }

    public async Task UpdateFundAsync(Guid id, UpdateFundCommand command)
    {
        await UpdateAsync($"api/funds/{id}", command);
    }

    public async Task DeleteFundAsync(Guid id)
    {
        await DeleteAsync($"api/funds/{id}");
    }
}