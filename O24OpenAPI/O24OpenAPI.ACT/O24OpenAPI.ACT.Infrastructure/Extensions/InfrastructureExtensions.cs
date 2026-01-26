using Microsoft.Extensions.DependencyInjection;

namespace O24OpenAPI.ACT.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddACTInfrastructureService(this IServiceCollection services)
    {
        services.AddLinKitDependency();
        return services;
    }
}
