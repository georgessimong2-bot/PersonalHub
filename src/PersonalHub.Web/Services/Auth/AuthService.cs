using Microsoft.JSInterop;
using PersonalHub.Web.Components.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace PersonalHub.Web.Services.Auth;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private readonly TokenStore _store;
    private readonly ILogger<AuthService> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string? AccessToken => _store.Token;

    public event Action? OnAuthStateChanged;

    public AuthService(
        IHttpClientFactory factory,
        IJSRuntime js,
        TokenStore store,
        ILogger<AuthService> logger)
    {
        _http = factory.CreateClient("Api");
        _js = js;
        _store = store;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var token = await _js.InvokeAsync<string?>("authStorage.get", "token");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _store.Token = token;
            _logger.LogInformation("Session restaurée depuis le stockage local.");
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

    public async Task<RegisterResult> RegisterAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", new
        {
            email,
            password
        });

        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return new RegisterResult { Success = true };

        try
        {
            var result = JsonSerializer.Deserialize<RegisterResult>(content, _jsonOptions);

            if (result is not null)
            {
                result.Success = false;
                return result;
            }
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Impossible de désérialiser la réponse d'inscription.");
        }

        return new RegisterResult { Success = false, Error = content };
    }

    public async Task SetToken(string token)
    {
        _store.Token = token;
        // Le header HTTP est injecté automatiquement par AuthHeaderHandler
        await _js.InvokeVoidAsync("authStorage.set", "token", token);
    }

    public string? GetToken() => _store.Token;

    /// <summary>
    /// Vérifie que le token est présent ET non expiré.
    /// </summary>
    public bool IsAuthenticated()
    {
        var jwt = GetParsedToken();
        return jwt is not null && jwt.ValidTo > DateTime.UtcNow;
    }

    public string? GetUserRole()
        => GetParsedToken()?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

    public string? GetUserEmail()
        => GetParsedToken()?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value;

    public string? GetUserId()
        => GetParsedToken()?.Claims
            .FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type.Contains("nameidentifier"))?.Value;

    public async Task LogoutAsync()
    {
        _store.Token = null;
        await _js.InvokeVoidAsync("authStorage.remove", "token");
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
        => OnAuthStateChanged?.Invoke();

    /// <summary>
    /// Parse le JWT une seule fois et le retourne. Retourne null si absent ou invalide.
    /// </summary>
    private JwtSecurityToken? GetParsedToken()
    {
        if (string.IsNullOrWhiteSpace(_store.Token))
            return null;

        try
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(_store.Token);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Impossible de parser le token JWT.");
            return null;
        }
    }
}