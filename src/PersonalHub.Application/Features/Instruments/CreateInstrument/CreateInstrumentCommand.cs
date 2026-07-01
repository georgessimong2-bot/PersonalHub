using MediatR;

namespace PersonalHub.Application.Features.Instruments.CreateInstrument;

public class CreateInstrumentCommand : IRequest<Guid>
{
    public Guid InstrumentTypeId { get; set; }

    public Guid CurrencyId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ISIN { get; set; } = string.Empty;

    public string? Ticker { get; set; }

    public string? Issuer { get; set; }

    public string? CountryOfRisk { get; set; }

    public string? Sector { get; set; }

    public bool IsActive { get; set; } = true;
}
