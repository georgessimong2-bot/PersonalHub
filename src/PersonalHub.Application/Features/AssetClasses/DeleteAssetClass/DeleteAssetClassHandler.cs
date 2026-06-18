using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Application.Features.AssetClasses.DeleteAssetClass;

public class DeleteAssetClassHandler
    : IRequestHandler<DeleteAssetClassCommand>
{
    private readonly IAppDbContext _context;

    public DeleteAssetClassHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteAssetClassCommand request,
        CancellationToken cancellationToken)
    {
        var assetClass = await _context.AssetClasses
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (assetClass is null)
            throw new Exception("Asset Class not found");

        _context.AssetClasses.Remove(assetClass);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
