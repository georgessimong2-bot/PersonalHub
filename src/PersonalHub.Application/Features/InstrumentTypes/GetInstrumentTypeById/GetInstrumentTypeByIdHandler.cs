using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.InstrumentTypes.Common;

namespace PersonalHub.Application.Features.InstrumentTypes.GetInstrumentTypeById;

public class GetInstrumentTypeByIdHandler
    : IRequestHandler<GetInstrumentTypeByIdQuery, InstrumentTypeDto?>
{
    private readonly IAppDbContext _context;

    public GetInstrumentTypeByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InstrumentTypeDto?> Handle(
        GetInstrumentTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.InstrumentTypes
            .Where(x => x.Id == request.Id)
            .Select(x => new InstrumentTypeDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
