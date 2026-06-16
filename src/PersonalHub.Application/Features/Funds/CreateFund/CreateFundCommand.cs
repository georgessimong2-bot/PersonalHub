using MediatR;

namespace PersonalHub.Application.Features.Funds.CreateFund;

public record CreateFundCommand(
    string Name,
    string? LegalName,
    string? FundCode,
    string? DomicileCountry,
    string? BaseCurrency,
    DateTime? LaunchDate,
    bool IsActive,
    string? Description,
    Guid FundTypeId
) : IRequest<Guid>;