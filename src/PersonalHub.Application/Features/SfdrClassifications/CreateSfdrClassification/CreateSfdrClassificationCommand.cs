using MediatR;

namespace PersonalHub.Application.Features.SfdrClassifications.CreateSfdrClassification;

public class CreateSfdrClassificationCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
