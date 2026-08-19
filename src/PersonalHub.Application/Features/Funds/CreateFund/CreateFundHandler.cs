using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Exceptions;
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
        if (request.FundTypeId == Guid.Empty)
            throw new BusinessException("Fund type is required.");

        var fundTypeExists = await _context.FundTypes
            .AnyAsync(x => x.Id == request.FundTypeId, cancellationToken);

        if (!fundTypeExists)
            throw new BusinessException("Selected fund type does not exist.");

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
