using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.AssetClasses.UpdateAssetClass;

public class UpdateAssetClassHandler
    : IRequestHandler<UpdateAssetClassCommand>
{
    private readonly IAppDbContext _context;

    public UpdateAssetClassHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateAssetClassCommand request,
        CancellationToken cancellationToken)
    {
        var assetClass = await _context.AssetClasses
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (assetClass is null)
            throw new Exception("Asset Class not found");

        assetClass.Name = request.Name;
        assetClass.Description = request.Description;
        assetClass.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
