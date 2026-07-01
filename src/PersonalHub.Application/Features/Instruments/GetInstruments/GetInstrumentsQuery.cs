using MediatR;
using PersonalHub.Application.Features.Instruments.Common;

namespace PersonalHub.Application.Features.Instruments.GetInstruments;

public record GetInstrumentsQuery()
    : IRequest<List<InstrumentDto>>;
