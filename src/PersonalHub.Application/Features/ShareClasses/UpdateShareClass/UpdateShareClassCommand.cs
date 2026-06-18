using MediatR;

namespace PersonalHub.Application.Features.ShareClasses.UpdateShareClass;

public record UpdateShareClassCommand(
    Guid Id,
    string Name,
    string ISIN,
    bool IsHedged,
    bool IsDistribution,
    bool IsInstitutional,
    decimal? ManagementFee,
    decimal? PerformanceFee,
    decimal? MinimumInvestment,
    DateTime? LaunchDate,
    bool IsActive)
    : IRequest;
