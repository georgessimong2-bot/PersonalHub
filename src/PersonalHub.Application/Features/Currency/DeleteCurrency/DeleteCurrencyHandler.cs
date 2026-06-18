using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.Currency.DeleteCurrency;

public class DeleteCurrencyHandler
    : IRequestHandler<DeleteCurrencyCommand>
{
    private readonly IAppDbContext _context;

    public DeleteCurrencyHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var currency = await _context.Currencies
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (currency is null)
            throw new Exception("Currency not found.");

        _context.Currencies.Remove(currency);

        await _context.SaveChangesAsync(cancellationToken);
    }
}