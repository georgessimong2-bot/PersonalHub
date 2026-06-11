using MediatR;

namespace PersonalHub.Application.Features.Goals.DeleteGoal;

public record DeleteGoalCommand(Guid Id)
    : IRequest;