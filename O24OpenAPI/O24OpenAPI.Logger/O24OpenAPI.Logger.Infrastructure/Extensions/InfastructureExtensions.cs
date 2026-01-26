using Microsoft.Extensions.DependencyInjection;

namespace O24OpenAPI.Logger.Infrastructure.Extensions;

public static class InfastructureExtensions
{
    public static IServiceCollection AddWFOInfrastructureServices(this IServiceCollection services)
    {
        // Register other infrastructure services here if needed
        services.AddLinKitDependency();
        return services;
    }
}
