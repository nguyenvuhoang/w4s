using O24OpenAPI.WFO.API.Application.Models;

namespace O24OpenAPI.WFO.API.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<WorkflowExecution>();
        services.AddLinKitDependency().AddLinKitCqrs();

        return services;
    }
}
