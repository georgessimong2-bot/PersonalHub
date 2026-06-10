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
        var token = _store.Token;
        Console.WriteLine("TOKEN = " + token);

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        Console.WriteLine("TOKEN LENGTH = " + (token?.Length ?? 0));
        Console.WriteLine("AUTH HEADER = " + request.Headers.Authorization);

        return base.SendAsync(request, cancellationToken);
    }
}