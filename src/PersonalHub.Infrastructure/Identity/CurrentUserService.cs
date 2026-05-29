using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Infrastructure.Identity;

public class CurrentUserService
    : ICurrentUserService
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
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? string.Empty;
}