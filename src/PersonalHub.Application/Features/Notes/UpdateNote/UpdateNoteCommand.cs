using MediatR;

namespace PersonalHub.Application.Features.Notes.UpdateNote;

public record UpdateNoteCommand(
    Guid Id,
    string Title,
    string Content)
    : IRequest;
