using MediatR;
using PersonalHub.Application.Features.InstrumentTypes.Common;

namespace PersonalHub.Application.Features.InstrumentTypes.GetInstrumentTypeById;

public record GetInstrumentTypeByIdQuery(Guid Id)
    : IRequest<InstrumentTypeDto?>;
