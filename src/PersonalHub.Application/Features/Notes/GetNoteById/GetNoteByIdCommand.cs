using MediatR;
using PersonalHub.Application.Features.Notes.Common;

namespace PersonalHub.Application.Features.Notes.GetNoteById;

public record GetNoteByIdCommand(Guid Id)
    : IRequest<NoteDto?>;