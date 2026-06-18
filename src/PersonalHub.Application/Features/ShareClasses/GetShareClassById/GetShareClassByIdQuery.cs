using MediatR;
using PersonalHub.Application.Features.ShareClasses.Common;

namespace PersonalHub.Application.Features.ShareClasses.GetShareClassById;

public record GetShareClassByIdQuery(Guid Id)
    : IRequest<ShareClassDto?>;
