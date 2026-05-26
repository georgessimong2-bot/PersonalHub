using MediatR;

namespace PersonalHub.Application.Features.Notes.DeleteNote;

public record DeleteNoteCommand(Guid Id)
    : IRequest;