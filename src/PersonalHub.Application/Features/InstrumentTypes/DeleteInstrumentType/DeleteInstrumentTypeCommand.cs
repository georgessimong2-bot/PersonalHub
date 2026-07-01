using MediatR;

namespace PersonalHub.Application.Features.InstrumentTypes.DeleteInstrumentType;

public record DeleteInstrumentTypeCommand(Guid Id)
    : IRequest;
