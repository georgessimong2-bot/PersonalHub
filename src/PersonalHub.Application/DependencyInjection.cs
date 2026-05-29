using Microsoft.Extensions.DependencyInjection;

namespace PersonalHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // MediatR is already configured in the Api project
        // This method is a placeholder for future application-level configurations

        return services;
    }
}
