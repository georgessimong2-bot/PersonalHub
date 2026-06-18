using MediatR;

namespace PersonalHub.Application.Features.Currency.CreateCurrency;

public class CreateCurrencyCommand : IRequest<Guid>
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;
}