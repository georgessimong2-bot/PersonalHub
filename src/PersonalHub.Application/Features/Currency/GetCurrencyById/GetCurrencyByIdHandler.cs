using global::PersonalHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Features.Currency.Common;

namespace PersonalHub.Application.Features.Currency.GetCurrencyById;

public class GetCurrencyByIdHandler
    : IRequestHandler<GetCurrencyByIdCommand, CurrencyDto?>
{
    private readonly IAppDbContext _context;

    public GetCurrencyByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CurrencyDto?> Handle(
        GetCurrencyByIdCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.Currencies
            .Where(x => x.Id == request.Id)
            .Select(x => new CurrencyDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Symbol = x.Symbol,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}