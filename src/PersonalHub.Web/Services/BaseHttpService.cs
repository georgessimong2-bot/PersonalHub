namespace PersonalHub.Web.Services;

/// <summary>
/// Base class for HTTP services providing standardized CRUD operations
/// with consistent error handling and authentication integration.
/// </summary>
public abstract class BaseHttpService
{
    protected readonly HttpClient Http;

    protected BaseHttpService(IHttpClientFactory factory)
    {
        Http = factory.CreateClient("Api");
    }

    /// <summary>
    /// Extracts error message from HTTP error response content.
    /// Supports both "error" and "message" fields in JSON response.
    /// </summary>
    private async Task<string> ExtractErrorMessageAsync(HttpContent content)
    {
        try
        {
            var errorResponse = await content.ReadFromJsonAsync<Dictionary<string, object>>();

            if (errorResponse?.ContainsKey("error") == true)
                return errorResponse["error"]?.ToString() ?? "Unknown error";

            if (errorResponse?.ContainsKey("message") == true)
                return errorResponse["message"]?.ToString() ?? "Unknown error";

            return "Unknown error";
        }
        catch
        {
            return "Unknown error";
        }
    }

    /// <summary>
    /// Gets all entities of type T from the specified endpoint.
    /// </summary>
    /// <typeparam name="T">The DTO type to deserialize</typeparam>
    /// <param name="endpoint">API endpoint (e.g., "api/users")</param>
    /// <returns>List of entities or empty list if not found</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task<List<T>> GetAllAsync<T>(string endpoint)
        where T : class
    {
        var response = await Http.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<List<T>>() ?? [];
    }

    /// <summary>
    /// Gets a single entity by ID from the specified endpoint.
    /// </summary>
    /// <typeparam name="T">The DTO type to deserialize</typeparam>
    /// <param name="endpoint">API endpoint (e.g., "api/users/123")</param>
    /// <returns>The entity or null if not found</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails (except 404)</exception>
    protected async Task<T?> GetByIdAsync<T>(string endpoint)
        where T : class
    {
        try
        {
            var response = await Http.GetAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ExtractErrorMessageAsync(response.Content);
                throw new HttpRequestException(errorMessage, null, response.StatusCode);
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new entity by posting to the specified endpoint.
    /// </summary>
    /// <typeparam name="TRequest">The command/request type</typeparam>
    /// <typeparam name="TResponse">The response type (usually Guid for the created ID)</typeparam>
    /// <param name="endpoint">API endpoint (e.g., "api/users")</param>
    /// <param name="request">The command/request object to post</param>
    /// <returns>The response from the server (typically the created entity's ID)</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task<TResponse?> CreateAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await Http.PostAsJsonAsync(endpoint, request);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    /// <summary>
    /// Creates a new entity and returns its ID.
    /// </summary>
    /// <typeparam name="TRequest">The command/request type</typeparam>
    /// <param name="endpoint">API endpoint (e.g., "api/users")</param>
    /// <param name="request">The command/request object to post</param>
    /// <returns>The ID of the created entity</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task<Guid> CreateAsync<TRequest>(
        string endpoint,
        TRequest request)
        where TRequest : class
    {
        var response = await Http.PostAsJsonAsync(endpoint, request);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    /// <summary>
    /// Updates an existing entity by putting to the specified endpoint.
    /// </summary>
    /// <typeparam name="TRequest">The command/request type</typeparam>
    /// <param name="endpoint">API endpoint (e.g., "api/users/123")</param>
    /// <param name="request">The command/request object to put</param>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task UpdateAsync<TRequest>(
        string endpoint,
        TRequest request)
        where TRequest : class
    {
        var response = await Http.PutAsJsonAsync(endpoint, request);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }
    }

    /// <summary>
    /// Deletes an entity by deleting from the specified endpoint.
    /// </summary>
    /// <param name="endpoint">API endpoint (e.g., "api/users/123")</param>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task DeleteAsync(string endpoint)
    {
        var response = await Http.DeleteAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }
    }

    /// <summary>
    /// Performs a generic POST request that returns a specific response type.
    /// Useful for custom operations like "increment" or "generate".
    /// </summary>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <param name="endpoint">API endpoint (e.g., "api/goals/123/increment")</param>
    /// <returns>The response from the server</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task<TResponse?> PostAsync<TResponse>(string endpoint)
        where TResponse : class
    {
        var response = await Http.PostAsync(endpoint, null);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    /// <summary>
    /// Performs a generic POST request with a request body that returns a specific response type.
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <param name="endpoint">API endpoint</param>
    /// <param name="request">The request object</param>
    /// <returns>The response from the server</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request)
        where TRequest : class
        where TResponse : class
    {
        var response = await Http.PostAsJsonAsync(endpoint, request);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    /// <summary>
    /// Performs a generic GET request that returns a specific response type.
    /// Useful for custom queries or aggregations.
    /// </summary>
    /// <typeparam name="TResponse">The response type</typeparam>
    /// <param name="endpoint">API endpoint (e.g., "api/dashboard/stats")</param>
    /// <returns>The response from the server</returns>
    /// <exception cref="HttpRequestException">Thrown if the request fails</exception>
    protected async Task<TResponse?> GetAsync<TResponse>(string endpoint)
        where TResponse : class
    {
        var response = await Http.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await ExtractErrorMessageAsync(response.Content);
            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}
