using PersonalHub.Application.Features.Notes.Common;
using PersonalHub.Application.Features.Notes.CreateNote;
using PersonalHub.Application.Features.Notes.UpdateNote;

namespace PersonalHub.Web.Services;

public class NoteService : BaseHttpService
{
    public NoteService(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<List<NoteDto>> GetNotesAsync()
    {
        return await GetAllAsync<NoteDto>("api/notes");
    }

    public async Task<NoteDto?> GetNoteByIdAsync(Guid id)
    {
        return await GetByIdAsync<NoteDto>($"api/notes/{id}");
    }

    public async Task<Guid> CreateNoteAsync(CreateNoteCommand command)
    {
        return await CreateAsync("api/notes", command);
    }

    public async Task UpdateNoteAsync(Guid id, UpdateNoteCommand command)
    {
        await UpdateAsync($"api/notes/{id}", command);
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        await DeleteAsync($"api/notes/{id}");
    }
}