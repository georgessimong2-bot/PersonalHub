using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Funds.Common;

namespace PersonalHub.Application.Features.Funds.GetFundById;

public class GetFundByIdHandler
    : IRequestHandler<GetFundByIdCommand, FundDto?>
{
    private readonly IAppDbContext _context;

    public GetFundByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<FundDto?> Handle(
        GetFundByIdCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Funds
            .Include(x => x.FundType)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return null;

        return new FundDto
        {
            Id = entity.Id,
            Name = entity.Name,
            FundTypeId = entity.FundTypeId,
            FundTypeName = entity.FundType.Name
        };
    }
}