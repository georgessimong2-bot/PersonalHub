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
        try
        {
            _logger.LogInformation("Starting upload for file: {FileName}, Size: {Size}", file.Name, file.Size);

            using var content = new MultipartFormDataContent();

            // Read file into byte array
            using var ms = new MemoryStream();
            await file.OpenReadStream(2 * 1024 * 1024).CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            _logger.LogInformation("File read into memory. Byte count: {ByteCount}", fileBytes.Length);

            // Use ByteArrayContent instead of StreamContent
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    string.IsNullOrWhiteSpace(file.ContentType) 
                        ? "application/octet-stream" 
                        : file.ContentType);

            _logger.LogInformation("Content-Type set to: {ContentType}", fileContent.Headers.ContentType);

            content.Add(fileContent, "file", file.Name);

            _logger.LogInformation("Sending request to api/account/profile-picture");

            var response = await _http.PostAsync("api/account/profile-picture", content);

            _logger.LogInformation("Response status: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                _logger.LogError(
                    "Failed to upload profile picture. Status: {Status}, Response: {Error}",
                    response.StatusCode, error);

                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<UploadResponse>();

            _logger.LogInformation("Upload succeeded. URL: {Url}", result?.Url);

            return result?.Url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while uploading profile picture.");
            return null;
        }
    }

    private sealed record UploadResponse(string? Url);
}