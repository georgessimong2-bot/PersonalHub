namespace PersonalHub.Application.Features.AI;

public class AiGoalAdvice
{
    public string Summary { get; set; } = string.Empty;

    public List<string> KeyInsights { get; set; } = [];

    public List<string> Actions { get; set; } = [];

    public string? Warning { get; set; }

    public int ConfidenceScore { get; set; } // 0–100
}