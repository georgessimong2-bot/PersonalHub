using MediatR;

namespace PersonalHub.Application.Features.Goals.IncrementGoal;

public record IncrementGoalCommand(Guid GoalId) : IRequest;