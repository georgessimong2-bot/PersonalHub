using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PersonalHub.Web.Services.Auth;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthService _authService;
    private readonly IJSRuntime _js;

    public JwtAuthenticationStateProvider(AuthService authService, IJSRuntime js)
    {
        _authService = authService;
        _js = js;
        _authService.OnAuthStateChanged += NotifyUserChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Console.WriteLine("AUTH PROVIDER CALLED");

        var token = _authService.GetToken();

        // 🔥 si mémoire vide → fallback sessionStorage
        if (string.IsNullOrWhiteSpace(token))
        {
            token = await _js.InvokeAsync<string>("authStorage.get", "token");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var claims = jwt.Claims
            .Select(c => new Claim(NormalizeType(c.Type), c.Value))
            .ToList();

        var identity = new ClaimsIdentity(
            claims,
            "jwt",
            ClaimTypes.Name,
            ClaimTypes.Role);

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private string NormalizeType(string type)
    {
        return type switch
        {
            "role" => ClaimTypes.Role,
            "roles" => ClaimTypes.Role,
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" => ClaimTypes.Role,
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role" => ClaimTypes.Role,
            _ => type
        };
    }

    private void NotifyUserChanged()
    {
        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());
    }
}