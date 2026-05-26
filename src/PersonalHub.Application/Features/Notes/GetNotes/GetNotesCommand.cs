using MediatR;
using PersonalHub.Application.Features.Notes.Common;

namespace PersonalHub.Application.Features.Notes.GetNotes;

public record GetNotesCommand()
    : IRequest<List<NoteDto>>;