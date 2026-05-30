using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using PersonalHub.Web.Configuration;

namespace PersonalHub.Web.Services.Auth;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private readonly ApiSettings _apiSettings;

    private const string TokenKey = "authToken";

    public AuthService(
        HttpClient http,
        IJSRuntime js,
        IOptions<ApiSettings> apiSettings)
    {
        _http = http;
        _js = js;
        _apiSettings = apiSettings.Value;
    }

    // LOGIN
    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync(
            $"{_apiSettings.BaseUrl}/api/auth/login",
            new { Email = email, Password = password });

        if (!response.IsSuccessStatusCode)
            return false;

        var token = await response.Content.ReadAsStringAsync();
        token = token.Replace("\"", "");

        await SetTokenAsync(token);

        return true;
    }

    // LOGOUT
    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        _http.DefaultRequestHeaders.Authorization = null;
    }

    // INIT au chargement app
    public async Task InitializeAsync()
    {
        var token = await GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // SET TOKEN
    private async Task SetTokenAsync(string token)
    {
        await _js.InvokeVoidAsync(
            "localStorage.setItem",
            TokenKey,
            token);

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    // GET TOKEN
    private async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>(
            "localStorage.getItem",
            TokenKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();

        return !string.IsNullOrWhiteSpace(token);
    }
}