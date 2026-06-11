using MediatR;
using PersonalHub.Application.Features.Goals.Common;

namespace PersonalHub.Application.Features.Goals.GetGoalStatistics;

public record GetGoalStatisticsCommand()
    : IRequest<GoalStatisticsDto>;