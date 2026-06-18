using System.Diagnostics;
using PersonalHub.Web.Services.Auth;

namespace PersonalHub.Web.HttpHandlers;

/// <summary>
/// A delegating handler that logs all HTTP requests/responses for debugging purposes.
/// Also handles 401 Unauthorized responses by triggering automatic logout.
/// </summary>
public class LoggingHttpHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHttpHandler> _logger;
    private readonly AuthService _authService;

    public LoggingHttpHandler(ILogger<LoggingHttpHandler> logger, AuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        _logger.LogInformation(
            "HTTP {Method} {Uri}",
            request.Method.Method,
            request.RequestUri);

        // Log authorization header presence
        if (request.Headers.Authorization != null)
        {
            _logger.LogInformation(
                "  → Authorization header present: {Scheme} token ({Length} chars)",
                request.Headers.Authorization.Scheme,
                request.Headers.Authorization.Parameter?.Length ?? 0);
        }
        else
        {
            _logger.LogWarning("  → No Authorization header!");
        }

        HttpResponseMessage response;
        try
        {
            response = await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "HTTP {Method} {Uri} - EXCEPTION after {ElapsedMs}ms",
                request.Method.Method,
                request.RequestUri,
                sw.ElapsedMilliseconds);
            throw;
        }

        sw.Stop();

        _logger.LogInformation(
            "HTTP {Method} {Uri} → {StatusCode} ({StatusDescription}) in {ElapsedMs}ms",
            request.Method.Method,
            request.RequestUri,
            (int)response.StatusCode,
            response.StatusCode,
            sw.ElapsedMilliseconds);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning(
                "HTTP {Method} {Uri} response body (first 500 chars): {Content}",
                request.Method.Method,
                request.RequestUri,
                content.Substring(0, Math.Min(500, content.Length)));

            // Handle 401 Unauthorized by triggering logout
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning(
                    "HTTP 401 Unauthorized received. Token might be expired. Triggering logout.");

                // This is fire-and-forget, but the next request from Blazor will see the empty token
                _ = _authService.LogoutAsync().ConfigureAwait(false);
            }
        }

        return response;
    }
}

