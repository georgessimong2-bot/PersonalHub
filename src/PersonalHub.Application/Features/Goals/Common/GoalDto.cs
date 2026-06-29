public class GoalDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal TargetValue { get; set; }

    public decimal CurrentValue { get; set; }

    public DateTime? Deadline { get; set; }

    public decimal ProgressPercentage { get; set; }

    public string Status { get; set; } = string.Empty;

    public int? DaysRemaining { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string? GeneratedAdvice { get; set; }
}