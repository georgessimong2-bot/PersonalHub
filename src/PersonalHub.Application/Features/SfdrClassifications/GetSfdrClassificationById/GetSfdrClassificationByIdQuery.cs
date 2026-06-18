using MediatR;
using PersonalHub.Application.Features.SfdrClassifications.Common;

namespace PersonalHub.Application.Features.SfdrClassifications.GetSfdrClassificationById;

public record GetSfdrClassificationByIdQuery(Guid Id)
    : IRequest<SfdrClassificationDto?>;
