using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.FundTypes.Common;

namespace PersonalHub.Application.Features.FundTypes.GetFundTypeById;

public class GetFundTypeByIdHandler
    : IRequestHandler<GetFundTypeByIdCommand, FundTypeDto?>
{
    private readonly IAppDbContext _context;

    public GetFundTypeByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<FundTypeDto?> Handle(
        GetFundTypeByIdCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.FundTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return null;

        return new FundTypeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}