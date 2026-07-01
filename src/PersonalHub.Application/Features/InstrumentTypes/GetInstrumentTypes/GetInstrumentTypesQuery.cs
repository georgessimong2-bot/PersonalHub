using MediatR;
using PersonalHub.Application.Features.InstrumentTypes.Common;

namespace PersonalHub.Application.Features.InstrumentTypes.GetInstrumentTypes;

public record GetInstrumentTypesQuery()
    : IRequest<List<InstrumentTypeDto>>;
