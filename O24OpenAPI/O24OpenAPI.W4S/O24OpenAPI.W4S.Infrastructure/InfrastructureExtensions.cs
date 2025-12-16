using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using O24OpenAPI.Logging.Interceptors;

namespace O24OpenAPI.W4S.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        services.ConfigureApplicationServices(builder);
        builder.ConfigureWebHost();
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcLoggingInterceptor>();
        });
        return services;
    }

    public static async Task ConfigureInfrastructure(this IApplicationBuilder app)
    {
        app.ConfigureRequestPipeline();
        await app.StartEngine();
    }
}
