using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

/// <summary>
/// NoOp Authentication Scheme - Required for Blazor Server with JWT client-side
/// </summary>
public class NoOpAuthenticationSchemeOptions : AuthenticationSchemeOptions { }

public class NoOpAuthenticationHandler : AuthenticationHandler<NoOpAuthenticationSchemeOptions>
{
    public NoOpAuthenticationHandler(
        IOptionsMonitor<NoOpAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
