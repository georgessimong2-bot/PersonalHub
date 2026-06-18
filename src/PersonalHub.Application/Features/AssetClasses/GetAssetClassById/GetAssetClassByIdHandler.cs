using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.AssetClasses.Common;

namespace PersonalHub.Application.Features.AssetClasses.GetAssetClassById;

public class GetAssetClassByIdHandler
    : IRequestHandler<GetAssetClassByIdQuery, AssetClassDto?>
{
    private readonly IAppDbContext _context;

    public GetAssetClassByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AssetClassDto?> Handle(
        GetAssetClassByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.AssetClasses
            .Where(x => x.Id == request.Id)
            .Select(x => new AssetClassDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
