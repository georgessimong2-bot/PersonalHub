using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.FundTypes.Common;

namespace PersonalHub.Application.Features.FundTypes.GetFundTypes;

public class GetFundTypesHandler
    : IRequestHandler<GetFundTypesCommand, List<FundTypeDto>>
{
    private readonly IAppDbContext _context;

    public GetFundTypesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FundTypeDto>> Handle(
        GetFundTypesCommand request,
        CancellationToken cancellationToken)
    {
        return await _context.FundTypes
            .Select(x => new FundTypeDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToListAsync(cancellationToken);
    }
}