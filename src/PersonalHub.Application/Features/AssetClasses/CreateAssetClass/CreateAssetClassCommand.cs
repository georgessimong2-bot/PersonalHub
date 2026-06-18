using MediatR;

namespace PersonalHub.Application.Features.AssetClasses.CreateAssetClass;

public class CreateAssetClassCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
