using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.GrpcContracts.Interceptors;

namespace O24OpenAPI.CTH.Infrastructure;

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
