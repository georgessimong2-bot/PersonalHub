using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using PersonalHub.Web.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace PersonalHub.Web.Services.Auth;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ApiSettings _apiSettings;
    private readonly IJSRuntime _js;
    private readonly TokenStore _store;
    public string? AccessToken => _store.Token;

    public event Action? OnAuthStateChanged;

    public AuthService(
        IHttpClientFactory factory,
        IOptions<ApiSettings> apiSettings,
        IJSRuntime js,
        TokenStore store)
    {
        _http = factory.CreateClient("Api");
        _apiSettings = apiSettings.Value;
        _js = js;
        _store = store;
    }

    public async Task InitializeAsync()
    {
        var token = await _js.InvokeAsync<string?>("authStorage.get", "token");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _store.Token = token;

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", new
        {
            Email = email,
            Password = password
        });

        if (!response.IsSuccessStatusCode)
            return false;

        var token = (await response.Content.ReadAsStringAsync())
            .Replace("\"", "");

        await SetToken(token);

        NotifyStateChanged();

        return true;
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", new
        {
            email,
            password
        });

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return true;
    }

    public async Task SetToken(string token)
    {
        _store.Token = token;

        await _js.InvokeVoidAsync("authStorage.set", "token", token);

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public string? GetToken() => _store.Token;

    public bool IsAuthenticated()
        => !string.IsNullOrWhiteSpace(_store.Token);

    public string? GetUserRole()
    {
        if (string.IsNullOrWhiteSpace(_store.Token))
            return null;

        var jwt = new JwtSecurityTokenHandler()
            .ReadJwtToken(_store.Token);

        return jwt.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
    }

    public string? GetUserEmail()
    {
        if (string.IsNullOrWhiteSpace(_store.Token))
            return null;

        try
        {
            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(_store.Token);

            return jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Email ||
                c.Type == "email")?.Value;
        }
        catch
        {
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        _store.Token = null;

        await _js.InvokeVoidAsync("authStorage.remove", "token");

        _http.DefaultRequestHeaders.Authorization = null;

        NotifyStateChanged();
    }

    private void NotifyStateChanged()
        => OnAuthStateChanged?.Invoke();

    public string? GetUserId()
    {
        if (string.IsNullOrWhiteSpace(_store.Token))
            return null;

        var jwt = new JwtSecurityTokenHandler()
            .ReadJwtToken(_store.Token);

        return jwt.Claims.FirstOrDefault(c =>
            c.Type == "sub")?.Value;
    }
}