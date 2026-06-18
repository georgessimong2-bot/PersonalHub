using MediatR;

namespace PersonalHub.Application.Features.SubFunds.DeleteSubFund;

public record DeleteSubFundCommand(Guid Id)
    : IRequest;
