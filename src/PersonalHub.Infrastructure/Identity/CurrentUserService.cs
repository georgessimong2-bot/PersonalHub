using Microsoft.AspNetCore.Http;
using PersonalHub.Application.Common.Interfaces;
using System.Security.Claims;

namespace PersonalHub.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId =>
        _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value
        ?? throw new UnauthorizedAccessException(
            "User is not authenticated");
}