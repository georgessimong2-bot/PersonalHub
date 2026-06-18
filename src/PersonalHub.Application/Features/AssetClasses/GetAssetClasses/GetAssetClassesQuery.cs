using MediatR;
using PersonalHub.Application.Features.AssetClasses.Common;

namespace PersonalHub.Application.Features.AssetClasses.GetAssetClasses;

public record GetAssetClassesQuery()
    : IRequest<List<AssetClassDto>>;
