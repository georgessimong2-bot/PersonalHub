using PersonalHub.Application.Features.Notes.Common;

public class NotesService
{
    private readonly HttpClient _http;

    public NotesService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Api");
    }

    public async Task<List<NoteDto>?> GetNotesAsync()
        => await _http.GetFromJsonAsync<List<NoteDto>>("api/notes");

    public async Task<NoteDto?> GetNoteAsync(Guid id)
        => await _http.GetFromJsonAsync<NoteDto>($"api/notes/{id}");

    public async Task<bool> CreateAsync(string title, string content)
    {
        var response = await _http.PostAsJsonAsync(
            "api/notes",
            new { Title = title, Content = content });

        Console.WriteLine("Authorization header:");
        Console.WriteLine(_http.DefaultRequestHeaders.Authorization);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(Guid id, string title, string content)
    {
        var response = await _http.PutAsJsonAsync(
            $"api/notes/{id}",
            new { Id = id, Title = title, Content = content });

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/notes/{id}");
        return response.IsSuccessStatusCode;
    }
}