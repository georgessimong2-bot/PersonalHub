using Microsoft.AspNetCore.Components.Forms;
using PersonalHub.Application.Features.Account.Common;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Application.Features.Users.CreateUser;
using PersonalHub.Application.Features.Users.UpdateUser;
using System.Text.Json;

namespace PersonalHub.Web.Services;

public class UserService
{
    private readonly HttpClient _http;
    private readonly ILogger<UserService> _logger;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UserService(
        IHttpClientFactory factory,
        ILogger<UserService> logger)
    {
        _http = factory.CreateClient("Api");
        _logger = logger;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var json = await _http.GetStringAsync("api/users");

        return JsonSerializer.Deserialize<List<UserDto>>(json, _jsonOptions) ?? [];
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var response = await _http.GetAsync($"api/users/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>(_jsonOptions);
    }

    public async Task<bool> CreateAsync(CreateUserCommand request)
    {
        var response = await _http.PostAsJsonAsync("api/users", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(string id, UpdateUserCommand request)
    {
        var response = await _http.PutAsJsonAsync($"api/users/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var response = await _http.DeleteAsync($"api/users/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
    {
        var response = await _http.PutAsJsonAsync("api/account/profile", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var response = await _http.PutAsJsonAsync("api/account/password", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> UploadProfilePictureAsync(IBrowserFile file)
    {
        using var content = new MultipartFormDataContent();
        await using var stream = file.OpenReadStream(2 * 1024 * 1024);
        using var fileContent = new StreamContent(stream);

        content.Add(fileContent, "file", file.Name);

        var response = await _http.PostAsync("api/account/profile-picture", content);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "Échec de l'upload de la photo de profil. Statut : {Status}",
                response.StatusCode);
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<UploadResponse>();
        return result?.Url;
    }

    private sealed record UploadResponse(string? Url);
}