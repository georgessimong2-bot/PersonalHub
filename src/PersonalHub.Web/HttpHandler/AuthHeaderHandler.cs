using PersonalHub.Web.Services.Auth;
using System.Net.Http.Headers;

namespace PersonalHub.Web.HttpHandlers;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IServiceProvider _sp;

    public AuthHeaderHandler(IServiceProvider sp)
    {
        _sp = sp;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var store = _sp.GetRequiredService<TokenStore>();

        var token = store.Token;

        Console.WriteLine("AUTH HEADER ADDED");
        Console.WriteLine("TOKEN = " + token);

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}