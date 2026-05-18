using MediatR;

namespace PersonalHub.Application.Features.Notes.GetNotes;

public record GetNotesCommand()
    : IRequest<List<NoteDto>>;