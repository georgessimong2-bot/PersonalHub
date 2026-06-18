using MediatR;
using PersonalHub.Application.Features.SfdrClassifications.Common;

namespace PersonalHub.Application.Features.SfdrClassifications.GetSfdrClassifications;

public record GetSfdrClassificationsQuery()
    : IRequest<List<SfdrClassificationDto>>;
