using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.ExchangeRates.DeleteExchangeRate;

public class DeleteExchangeRateHandler : IRequestHandler<DeleteExchangeRateCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteExchangeRateHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteExchangeRateCommand request,
        CancellationToken cancellationToken)
    {
        var exchangeRate = await _context.ExchangeRates
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (exchangeRate == null)
        {
            throw new InvalidOperationException($"Exchange rate with ID {request.Id} not found.");
        }

        _context.ExchangeRates.Remove(exchangeRate);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
