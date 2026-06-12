using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Infrastructure.AI;
using PersonalHub.Infrastructure.Data;
using PersonalHub.Infrastructure.Identity;

namespace PersonalHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(
                    "DefaultConnection")));

        services
     .AddIdentityCore<AppUser>()
     .AddRoles<IdentityRole>()
     .AddEntityFrameworkStores<AppDbContext>()
     .AddSignInManager()
     .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IAiService, OpenAiService>();

        services.AddScoped<IAppDbContext>(
            provider =>
                provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IIdentityService,
            IdentityService>();



        return services;
    }
}