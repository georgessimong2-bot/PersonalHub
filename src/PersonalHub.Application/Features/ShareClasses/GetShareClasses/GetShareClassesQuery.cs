using MediatR;
using PersonalHub.Application.Features.ShareClasses.Common;

namespace PersonalHub.Application.Features.ShareClasses.GetShareClasses;

public record GetShareClassesQuery()
    : IRequest<List<ShareClassDto>>;
