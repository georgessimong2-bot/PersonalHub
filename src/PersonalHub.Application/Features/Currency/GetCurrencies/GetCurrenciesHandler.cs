using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Currency.Common;
using PersonalHub.Application.Features.Currency.GetCurrencies;

public class GetCurrenciesHandler
    : IRequestHandler<GetCurrenciesCommand, List<CurrencyDto>>
{
    private readonly IAppDbContext _context;

    public GetCurrenciesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CurrencyDto>> Handle(
        GetCurrenciesCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Currencies
            .Select(x => new CurrencyDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Symbol = x.Symbol,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}