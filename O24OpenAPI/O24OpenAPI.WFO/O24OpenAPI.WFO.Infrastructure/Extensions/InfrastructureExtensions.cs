using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.WFO.Infrastructure.Services.Queue;

namespace O24OpenAPI.WFO.Infrastructure.Extensions;

public static class RegisterQueue
{
    public static void AddQueue(this IApplicationBuilder app)
    {
        _ = QueueContext.Instance;
    }
}

public static class InfrastructureExtensions
{
    public static void UseWFOInfrastructure(this IApplicationBuilder app)
    {
        app.AddQueue();
    }
}

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddWFOInfrastructureServices(this IServiceCollection services)
    {
        // Register other infrastructure services here if needed
        services.AddLinKitDependency();
        return services;
    }
}
