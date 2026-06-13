using MediatR;

namespace PersonalHub.Application.Features.AI.GenerateGoalAdvice;

public record GenerateGoalAdviceCommand(
    Guid GoalId)
    : IRequest<AiGoalAdvice>;