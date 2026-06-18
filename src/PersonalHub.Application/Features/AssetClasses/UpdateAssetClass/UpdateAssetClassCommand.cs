using MediatR;

namespace PersonalHub.Application.Features.AssetClasses.UpdateAssetClass;

public record UpdateAssetClassCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive)
    : IRequest;
