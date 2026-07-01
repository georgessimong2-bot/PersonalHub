using MediatR;
using PersonalHub.Application.Features.Instruments.Common;

namespace PersonalHub.Application.Features.Instruments.GetInstrumentById;

public record GetInstrumentByIdQuery(Guid Id)
    : IRequest<InstrumentDto?>;
