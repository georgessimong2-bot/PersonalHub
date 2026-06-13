using MediatR;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Domain.Entities;

namespace PersonalHub.Application.Features.FundTypes.CreateFundType;

public class CreateFundTypeHandler
    : IRequestHandler<CreateFundTypeCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateFundTypeHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateFundTypeCommand request,
        CancellationToken cancellationToken)
    {
        var fundType = new FundType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        _context.FundTypes.Add(fundType);

        await _context.SaveChangesAsync(cancellationToken);

        return fundType.Id;
    }
}