using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalHub.Application.Features.Notes.UpdateNote;

public record UpdateNoteCommand(
    Guid Id,
    string Title,
    string Content)
    : IRequest;
