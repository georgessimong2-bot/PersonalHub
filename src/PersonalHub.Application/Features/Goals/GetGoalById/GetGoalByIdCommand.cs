using MediatR;
using PersonalHub.Application.Features.Goals.Common;

namespace PersonalHub.Application.Features.Goals.GetGoalById;

public record GetGoalByIdCommand(Guid Id)
    : IRequest<GoalDto>;