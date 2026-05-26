using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Notes.Common;

public static class NoteMapper
{
    public static NoteDto ToDto(this Note note)
    {
        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt
        };
    }

    public static List<NoteDto> ToDto(this IEnumerable<Note> notes)
    {
        return notes.Select(x => x.ToDto()).ToList();
    }
}
