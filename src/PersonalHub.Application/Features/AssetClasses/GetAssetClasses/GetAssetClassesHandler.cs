using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.AssetClasses.Common;

namespace PersonalHub.Application.Features.AssetClasses.GetAssetClasses;

public class GetAssetClassesHandler
    : IRequestHandler<GetAssetClassesQuery, List<AssetClassDto>>
{
    private readonly IAppDbContext _context;

    public GetAssetClassesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AssetClassDto>> Handle(
        GetAssetClassesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.AssetClasses
            .Select(x => new AssetClassDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
