using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.SfdrClassifications.Common;

namespace PersonalHub.Application.Features.SfdrClassifications.GetSfdrClassifications;

public class GetSfdrClassificationsHandler
    : IRequestHandler<GetSfdrClassificationsQuery, List<SfdrClassificationDto>>
{
    private readonly IAppDbContext _context;

    public GetSfdrClassificationsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SfdrClassificationDto>> Handle(
        GetSfdrClassificationsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.SfdrClassifications
            .Select(x => new SfdrClassificationDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToListAsync(cancellationToken);
    }
}
