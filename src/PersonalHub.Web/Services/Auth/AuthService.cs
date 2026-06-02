using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using PersonalHub.Web.Configuration;

namespace PersonalHub.Web.Services.Auth;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ApiSettings _apiSettings;

    private string? _token;

    public AuthService(
        HttpClient http,
        IOptions<ApiSettings> apiSettings)
    {
        _http = http;
        _apiSettings = apiSettings.Value;
    }

    #region LOGIN

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await _http.PostAsJsonAsync(
            $"{_apiSettings.BaseUrl}/api/auth/login",
            new
            {
                Email = email,
                Password = password
            });

        if (!response.IsSuccessStatusCode)
            return false;

        var token = (await response.Content.ReadAsStringAsync())
            .Replace("\"", "");

        SetToken(token);

        return true;
    }

    #endregion

    #region TOKEN MANAGEMENT

    public void SetToken(string token)
    {
        _token = token;

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public string? GetToken()
        => _token;

    #endregion

    #region AUTH STATE

    public bool IsAuthenticated()
        => !string.IsNullOrWhiteSpace(_token);

    public string? GetUserEmail()
    {
        if (string.IsNullOrWhiteSpace(_token))
            return null;

        try
        {
            var jwt = new JwtSecurityTokenHandler()
                .ReadJwtToken(_token);

            return jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Email ||
                c.Type == "email")?.Value;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region LOGOUT

    public void Logout()
    {
        _token = null;
        _http.DefaultRequestHeaders.Authorization = null;
    }

    #endregion
}