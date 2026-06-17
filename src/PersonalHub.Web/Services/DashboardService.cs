using PersonalHub.Application.Features.Dashboard.Common;

namespace PersonalHub.Web.Services;

public class DashboardService
{
    private readonly HttpClient _http;

    public DashboardService(
        IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<DashboardDto?> GetAsync()
    {
        return await _http
            .GetFromJsonAsync<DashboardDto>(
                "api/dashboard");
    }
}