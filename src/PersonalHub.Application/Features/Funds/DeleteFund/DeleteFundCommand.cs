using MediatR;

namespace PersonalHub.Application.Features.Funds.DeleteFund;

public record DeleteFundCommand(Guid Id)
    : IRequest;