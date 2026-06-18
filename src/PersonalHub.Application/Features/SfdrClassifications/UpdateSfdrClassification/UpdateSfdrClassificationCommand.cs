using MediatR;

namespace PersonalHub.Application.Features.SfdrClassifications.UpdateSfdrClassification;

public record UpdateSfdrClassificationCommand(
    Guid Id,
    string Name,
    string? Description)
    : IRequest;
