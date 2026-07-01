using MediatR;
using PersonalHub.Application.Features.InstrumentPrices.Common;

namespace PersonalHub.Application.Features.InstrumentPrices.GetInstrumentPricesByInstrumentId;

public record GetInstrumentPricesByInstrumentIdQuery(Guid InstrumentId)
    : IRequest<List<InstrumentPriceDto>>;
