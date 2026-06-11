using MediatR;

namespace PersonalHub.Application.Features.Goals.UpdateGoal;

public record UpdateGoalCommand(
    Guid Id,
    string Title,
    string Description,
    decimal TargetValue,
    decimal CurrentValue,
    DateTime? Deadline)
    : IRequest;