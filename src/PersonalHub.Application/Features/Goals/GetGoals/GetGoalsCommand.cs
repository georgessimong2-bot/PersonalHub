using MediatR;
using PersonalHub.Application.Features.Goals.Common;

namespace PersonalHub.Application.Features.Goals.GetGoals;

public record GetGoalsCommand()
    : IRequest<List<GoalDto>>;