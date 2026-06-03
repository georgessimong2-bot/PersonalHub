using PersonalHub.Application.Features.Users.Common;
using System.Text.Json;

public class UserService
{
    private readonly HttpClient _http;

    public UserService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var json = await _http.GetStringAsync("api/users");

        Console.WriteLine(json);

        return JsonSerializer.Deserialize<List<UserDto>>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? [];
    }
}