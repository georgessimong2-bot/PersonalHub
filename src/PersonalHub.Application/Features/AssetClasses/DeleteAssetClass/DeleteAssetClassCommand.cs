using MediatR;

namespace PersonalHub.Application.Features.AssetClasses.DeleteAssetClass;

public record DeleteAssetClassCommand(Guid Id)
    : IRequest;
