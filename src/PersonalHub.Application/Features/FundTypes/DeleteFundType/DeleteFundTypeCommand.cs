using MediatR;

namespace PersonalHub.Application.Features.FundTypes.DeleteFundType;

public record DeleteFundTypeCommand(Guid Id)
    : IRequest;