using Microsoft.AspNetCore.Components.Forms;
using PersonalHub.Application.Features.Account.Common;
using PersonalHub.Application.Features.Users.Common;
using PersonalHub.Application.Features.Users.CreateUser;
using PersonalHub.Application.Features.Users.UpdateUser;
using PersonalHub.Web.Services.Auth;
using System.Net.Http.Headers;
using System.Text.Json;

public class UserService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;

    public UserService(
     IHttpClientFactory factory,
     AuthService authService)
    {
        _http = factory.CreateClient("Api");
        _authService = authService;
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

    public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
    {
        ApplyToken();

        var response = await _http.PutAsJsonAsync(
            "api/account/profile",
            dto);

        var body = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ChangePasswordAsync(
    ChangePasswordDto dto)
    {
        ApplyToken();
        var response =
            await _http.PutAsJsonAsync(
                "api/account/password",
                dto);

        return response.IsSuccessStatusCode;
    }

    public async Task<string?> UploadProfilePictureAsync(
    IBrowserFile file)
    {
        ApplyToken();

        using var content =
            new MultipartFormDataContent();

        await using var stream =
            file.OpenReadStream(2 * 1024 * 1024);

        using var fileContent =
            new StreamContent(stream);

        content.Add(
            fileContent,
            "file",
            file.Name);

        var response =
            await _http.PostAsync(
                "api/account/profile-picture",
                content);

        Console.WriteLine($"STATUS = {response.StatusCode}");

        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"BODY = {body}");

        if (!response.IsSuccessStatusCode)
            return null;

        var result =
            await response.Content.ReadFromJsonAsync<UploadResponse>();

        return result?.Url;
    }

    private class UploadResponse
    {
        public string? Url { get; set; }
    }


    private void ApplyToken()
    {
        var token = _authService.GetToken();

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }
    }
}