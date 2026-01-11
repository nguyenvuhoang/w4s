using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.GrpcContracts.Interceptors;
using O24OpenAPI.Logging.Interceptors;

namespace O24OpenAPI.DWH.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcServerInboundInterceptor>();
        });
        services.AddLinKitDependency();
        return services;
    }

    public static async Task ConfigureInfrastructure(this IApplicationBuilder app)
    {
        app.ConfigureRequestPipeline();
        await app.StartEngine();
    }
}
