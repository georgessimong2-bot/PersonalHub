using MediatR;

namespace PersonalHub.Application.Features.SfdrClassifications.DeleteSfdrClassification;

public record DeleteSfdrClassificationCommand(Guid Id)
    : IRequest;
