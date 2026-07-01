using MediatR;

namespace PersonalHub.Application.Features.InstrumentPrices.UpdateInstrumentPrice;

public record UpdateInstrumentPriceCommand(
    Guid Id,
    decimal Price,
    DateTime PriceDate)
    : IRequest;
