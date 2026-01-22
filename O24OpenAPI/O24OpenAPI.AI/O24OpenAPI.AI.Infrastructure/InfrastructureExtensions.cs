using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using Qdrant.Client;

namespace O24OpenAPI.AI.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddLinKitDependency();
        QdrantSettingConfig? qdrantSettingConfig = Singleton<AppSettings>.Instance.Get<QdrantSettingConfig>();
        if (qdrantSettingConfig is not null)
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
