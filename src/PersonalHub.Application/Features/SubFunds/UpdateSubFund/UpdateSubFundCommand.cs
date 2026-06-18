using MediatR;

namespace PersonalHub.Application.Features.SubFunds.UpdateSubFund;

public record UpdateSubFundCommand(
    Guid Id,
    string Name,
    string? InternalCode,
    string? InvestmentObjective,
    string? InvestmentPolicy,
    string? GeographicFocus,
    string? SectorFocus,
    string? RiskProfile,
    string? Description)
    : IRequest;
