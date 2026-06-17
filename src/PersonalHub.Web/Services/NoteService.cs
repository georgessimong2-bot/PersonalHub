using PersonalHub.Application.Features.Notes.Common;
using PersonalHub.Web.Services.Auth;
using System.Net.Http.Headers;

public class NotesService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;

    public NotesService(
        IHttpClientFactory factory,
        AuthService authService)
    {
        _http = factory.CreateClient("Api");
        _authService = authService;
    }

    private void SetAuthorizationHeader()
    {
        var token = _authService.GetToken();

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task<List<NoteDto>?> GetNotesAsync()
    {
        SetAuthorizationHeader();

        return await _http.GetFromJsonAsync<List<NoteDto>>(
            "api/notes");
    }


    public async Task<NoteDto?> GetNoteByIdAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<NoteDto>(
            $"api/notes/{id}");
    }

    public async Task<bool> CreateAsync(
        string title,
        string content)
    {
        SetAuthorizationHeader();

        var response = await _http.PostAsJsonAsync(
            "api/notes",
            new
            {
                Title = title,
                Content = content
            });

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        string title,
        string content)
    {
        SetAuthorizationHeader();

        var response = await _http.PutAsJsonAsync(
            $"api/notes/{id}",
            new
            {
                Id = id,
                Title = title,
                Content = content
            });

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        SetAuthorizationHeader();

        var response = await _http.DeleteAsync(
            $"api/notes/{id}");

        return response.IsSuccessStatusCode;
    }
}