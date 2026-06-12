namespace PersonalHub.Application.Common.Interfaces;

public interface IAiService
{
    Task<string> GenerateGoalAdviceAsync(
        string prompt);
}