using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.SfdrClassifications.Common;

namespace PersonalHub.Application.Features.SfdrClassifications.GetSfdrClassificationById;

public class GetSfdrClassificationByIdHandler
    : IRequestHandler<GetSfdrClassificationByIdQuery, SfdrClassificationDto?>
{
    private readonly IAppDbContext _context;

    public GetSfdrClassificationByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<SfdrClassificationDto?> Handle(
        GetSfdrClassificationByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.SfdrClassifications
            .Where(x => x.Id == request.Id)
            .Select(x => new SfdrClassificationDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
