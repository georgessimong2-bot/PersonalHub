using MediatR;
using PersonalHub.Application.Common.Interfaces;
using CurrencyEntity = PersonalHub.Domain.Entities.Currency;

namespace PersonalHub.Application.Features.Currency.CreateCurrency;

public class CreateCurrencyHandler
    : IRequestHandler<CreateCurrencyCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateCurrencyHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var currency = new CurrencyEntity
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            Name = request.Name,
            Symbol = request.Symbol,
            IsActive = true
        };

        _context.Currencies.Add(currency);

        await _context.SaveChangesAsync(cancellationToken);

        return currency.Id;
    }
}