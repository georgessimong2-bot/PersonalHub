using MediatR;

namespace PersonalHub.Application.Features.InstrumentPrices.DeleteInstrumentPrice;

public record DeleteInstrumentPriceCommand(Guid Id) : IRequest;
