using MediatR;
using PersonalHub.Application.Features.Notes.GetNotes;

namespace PersonalHub.Application.Features.Notes.GetNoteById;

public record GetNoteByIdCommand(Guid Id)
    : IRequest<NoteDto?>;