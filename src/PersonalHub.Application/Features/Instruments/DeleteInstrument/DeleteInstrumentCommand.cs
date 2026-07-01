using MediatR;

namespace PersonalHub.Application.Features.Instruments.DeleteInstrument;

public record DeleteInstrumentCommand(Guid Id)
    : IRequest;
