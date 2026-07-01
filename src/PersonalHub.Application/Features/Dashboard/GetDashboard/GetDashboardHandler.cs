using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.Dashboard.Common;

namespace PersonalHub.Application.Features.Dashboard.GetDashboard;

public class GetDashboardHandler
    : IRequestHandler<GetDashboardCommand, DashboardDto>
{
    private readonly IAppDbContext _context;

    public GetDashboardHandler(
        IAppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> Handle(
        GetDashboardCommand request,
        CancellationToken cancellationToken)
    {
        return new DashboardDto
        {
            GoalsCount =
                await _context.Goals.CountAsync(cancellationToken),

            NotesCount =
                await _context.Notes.CountAsync(cancellationToken),

            FundsCount =
                await _context.Funds.CountAsync(cancellationToken),

            SubFundsCount =
                await _context.SubFunds.CountAsync(cancellationToken),

            ShareClassesCount =
                await _context.ShareClasses.CountAsync(cancellationToken)
        };
    }
}