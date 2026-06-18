namespace PersonalHub.Application.Features.Currency.Common;

public class CurrencyDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}