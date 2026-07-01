using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.InstrumentTypes.Common;

namespace PersonalHub.Application.Features.InstrumentTypes.GetInstrumentTypes;

public class GetInstrumentTypesHandler
    : IRequestHandler<GetInstrumentTypesQuery, List<InstrumentTypeDto>>
{
    private readonly IAppDbContext _context;

    public GetInstrumentTypesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InstrumentTypeDto>> Handle(
        GetInstrumentTypesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.InstrumentTypes
            .Select(x => new InstrumentTypeDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
