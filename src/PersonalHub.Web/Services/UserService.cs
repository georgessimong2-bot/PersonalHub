using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Application.Features.Users.CreateUser;
using PersonalHub.Application.Features.Users.UpdateUser;
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

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var response = await _http.GetAsync($"api/users/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<UserDto>();
    }

    public async Task<bool> CreateAsync(
        CreateUserCommand request)
    {
        var response = await _http.PostAsJsonAsync(
            "api/users",
            request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(
        string id,
        UpdateUserCommand request)
    {
        var response = await _http.PutAsJsonAsync(
            $"api/users/{id}",
            request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var response =
            await _http.DeleteAsync($"api/users/{id}");

        return response.IsSuccessStatusCode;
    }
}