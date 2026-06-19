using PersonalHub.Web.Services;

namespace PersonalHub.Web.Services;

public class AuthCheckService : BaseHttpService
{
    public AuthCheckService(IHttpClientFactory factory) : base(factory)
    {
    }

    public async Task<bool> IsAdminAsync()
    {
        try
        {
            var response = await Http.GetFromJsonAsync<AdminCheckResponse>("api/auth/check-admin");
            return response?.IsAdmin ?? false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private record AdminCheckResponse(bool IsAdmin);
}
