using MediatR;

namespace PersonalHub.Application.Features.Currency.UpdateCurrency;

public class UpdateCurrencyCommand : IRequest
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}