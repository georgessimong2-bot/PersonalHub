using MediatR;

namespace PersonalHub.Application.Features.Funds.UpdateFund;

public record UpdateFundCommand(
    Guid Id,
    string Name,
    string? LegalName,
    string? FundCode,
    string? DomicileCountry,
    string? BaseCurrency,
    DateTime? LaunchDate,
    bool IsActive,
    string? Description,
    Guid FundTypeId
) : IRequest;