using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Logging.Interceptors;
using O24OpenAPI.O24AI.Configuration;
using O24OpenAPI.Web.Framework.Infrastructure.Extensions;
using Qdrant.Client;

namespace O24OpenAPI.AI.Infrastructure;

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
        services.AddLinKitDependency();
        var qdrantSettingConfig = Singleton<AppSettings>.Instance.Get<QdrantSettingConfig>();
        services.AddSingleton(_ =>
        {
            return new QdrantClient(
                host: qdrantSettingConfig.Host,
                port: qdrantSettingConfig.Port
            );
        });
        return services;
    }

    public static async Task ConfigureInfrastructure(this IApplicationBuilder app)
    {
        app.ConfigureRequestPipeline();
        await app.StartEngine();
    }
}
