using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.AssetClasses.CreateAssetClass;

public class CreateAssetClassHandler
    : IRequestHandler<CreateAssetClassCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateAssetClassHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateAssetClassCommand request,
        CancellationToken cancellationToken)
    {
        var assetClass = new AssetClass
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        _context.AssetClasses.Add(assetClass);
        await _context.SaveChangesAsync(cancellationToken);

        return assetClass.Id;
    }
}
