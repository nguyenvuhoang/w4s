using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Framework.Infrastructure.Extensions;

namespace O24OpenAPI.EXT.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddLinKitDependency();
        return services;
    }

    public static async Task ConfigureInfrastructure(this IApplicationBuilder app)
    {
        app.ConfigureRequestPipeline();
        await app.StartEngine();
    }
}
