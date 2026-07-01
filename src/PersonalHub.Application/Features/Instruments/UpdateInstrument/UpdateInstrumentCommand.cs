using MediatR;

namespace PersonalHub.Application.Features.Instruments.UpdateInstrument;

public record UpdateInstrumentCommand(
    Guid Id,
    Guid InstrumentTypeId,
    Guid CurrencyId,
    string Name,
    string ISIN,
    string? Ticker,
    string? Issuer,
    string? CountryOfRisk,
    string? Sector,
    bool IsActive)
    : IRequest;
