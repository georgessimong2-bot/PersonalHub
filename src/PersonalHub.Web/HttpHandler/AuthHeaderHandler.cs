using PersonalHub.Web.Services.Auth;
using System.Net.Http.Headers;

namespace PersonalHub.Web.HttpHandlers;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly TokenStore _store;

    public AuthHeaderHandler(TokenStore store)
    {
        _store = store;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_store.Token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _store.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}