namespace PersonalHub.Application.Features.Currency.UpdateCurrency;

using global::PersonalHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


public class UpdateCurrencyHandler
    : IRequestHandler<UpdateCurrencyCommand>
{
    private readonly IAppDbContext _context;

    public UpdateCurrencyHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var currency = await _context.Currencies
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (currency is null)
            throw new Exception("Currency not found.");

        currency.Code = request.Code;
        currency.Name = request.Name;
        currency.Symbol = request.Symbol;
        currency.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}