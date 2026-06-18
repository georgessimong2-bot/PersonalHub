namespace PersonalHub.Domain.Entities;

public class SfdrClassification : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}