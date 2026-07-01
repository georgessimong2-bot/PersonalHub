using MediatR;

namespace PersonalHub.Application.Features.InstrumentTypes.UpdateInstrumentType;

public record UpdateInstrumentTypeCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive)
    : IRequest;
