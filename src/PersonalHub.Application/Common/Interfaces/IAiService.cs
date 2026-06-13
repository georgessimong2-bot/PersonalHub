using PersonalHub.Application.Features.AI;

namespace PersonalHub.Application.Common.Interfaces;

public interface IAiService
{
    Task<AiGoalAdvice> GenerateGoalAdviceAsync(string prompt);
}