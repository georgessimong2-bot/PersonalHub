namespace PersonalHub.Domain.Entities;

public class Goal
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal TargetValue { get; set; }

    public decimal CurrentValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? Deadline { get; set; }

    public string? GeneratedAdvice { get; set; }

    public string UserId { get; set; } = string.Empty;
}