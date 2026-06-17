using MediatR;
using PersonalHub.Application.Features.Dashboard.Common;

namespace PersonalHub.Application.Features.Dashboard.GetDashboard;

public record GetDashboardCommand : IRequest<DashboardDto>;