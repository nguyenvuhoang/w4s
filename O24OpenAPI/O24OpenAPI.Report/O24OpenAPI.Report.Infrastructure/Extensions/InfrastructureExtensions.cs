using Microsoft.Extensions.DependencyInjection;

namespace O24OpenAPI.Report.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddLinKitDependency();

        return services;
    }
}
