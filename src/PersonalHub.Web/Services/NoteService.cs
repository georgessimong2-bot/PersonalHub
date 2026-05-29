using Microsoft.Extensions.Options;
using PersonalHub.Application.Features.Notes.Common;
using PersonalHub.Web.Configuration;

namespace PersonalHub.Web.Services;

public class NotesService
{
    private readonly HttpClient _http;
    private readonly ApiSettings _apiSettings;

    public NotesService(
        HttpClient http,
        IOptions<ApiSettings> apiSettings)
    {
        _http = http;
        _apiSettings = apiSettings.Value;
    }

    // GET ALL
    public async Task<List<NoteDto>?> GetNotesAsync()
    {
        return await _http.GetFromJsonAsync<List<NoteDto>>(
            $"{_apiSettings.BaseUrl}/api/notes");
    }

    // GET BY ID
    public async Task<NoteDto?> GetNoteAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<NoteDto>(
            $"{_apiSettings.BaseUrl}/api/notes/{id}");
    }

    // CREATE
    public async Task<bool> CreateAsync(string title, string content)
    {
        var response = await _http.PostAsJsonAsync(
            $"{_apiSettings.BaseUrl}/api/notes",
            new { Title = title, Content = content });

        return response.IsSuccessStatusCode;
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Guid id, string title, string content)
    {
        var response = await _http.PutAsJsonAsync(
            $"{_apiSettings.BaseUrl}/api/notes/{id}",
            new { Id = id, Title = title, Content = content });

        return response.IsSuccessStatusCode;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _http.DeleteAsync(
            $"{_apiSettings.BaseUrl}/api/notes/{id}");

        return response.IsSuccessStatusCode;
    }
}