using MediatR;

namespace PersonalHub.Application.Features.Goals.CreateGoal;

public record CreateGoalCommand(
    string Title,
    string? Description,
    decimal TargetValue,
    DateTime? Deadline
) : IRequest<Guid>;