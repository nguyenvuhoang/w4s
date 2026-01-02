using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Logging.Interceptors;

namespace O24OpenAPI.NCH.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcLoggingInterceptor>();
        });
        services.AddLinKitDependency();
        return services;
    }
}