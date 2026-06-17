using PersonalHub.Web.Services.Auth;
using System.Net.Http.Headers;

namespace PersonalHub.Web.HttpHandlers;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly TokenStore _store;
    private readonly ILogger<AuthHeaderHandler> _logger;

    public AuthHeaderHandler(TokenStore store, ILogger<AuthHeaderHandler> logger)
    {
        _store = store;
        _logger = logger;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_store.Token))
        {
            _logger.LogInformation("AuthHeaderHandler: Adding Bearer token. Token length: {Length}", _store.Token.Length);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _store.Token);
        }
        else
        {
            _logger.LogWarning("AuthHeaderHandler: No token available in TokenStore!");
        }

        return base.SendAsync(request, cancellationToken);
    }
}