using MediatR;

namespace PersonalHub.Application.Features.ShareClasses.DeleteShareClass;

public record DeleteShareClassCommand(Guid Id)
    : IRequest;
