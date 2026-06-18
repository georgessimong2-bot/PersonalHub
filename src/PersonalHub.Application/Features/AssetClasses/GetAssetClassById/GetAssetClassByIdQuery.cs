using MediatR;
using PersonalHub.Application.Features.AssetClasses.Common;

namespace PersonalHub.Application.Features.AssetClasses.GetAssetClassById;

public record GetAssetClassByIdQuery(Guid Id)
    : IRequest<AssetClassDto?>;
