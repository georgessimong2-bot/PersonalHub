using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.Funds.CreateFund;

public class CreateFundHandler
    : IRequestHandler<CreateFundCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateFundHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateFundCommand request,
        CancellationToken cancellationToken)
    {
        var fund = new Fund
        {
            Id = Guid.NewGuid(),

            Name = request.Name,
            LegalName = request.LegalName,
            FundCode = request.FundCode,

            DomicileCountry = request.DomicileCountry,
            BaseCurrency = request.BaseCurrency,

            LaunchDate = request.LaunchDate,
            IsActive = request.IsActive,

            Description = request.Description,

            FundTypeId = request.FundTypeId
        };

        _context.Funds.Add(fund);

        await _context.SaveChangesAsync(cancellationToken);

        return fund.Id;
    }
}