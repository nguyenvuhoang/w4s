using O24OpenAPI.WFO.API.Application.Models;

namespace O24OpenAPI.WFO.API.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddpplicationServices(this IServiceCollection services)
    {
        services.AddScoped<WorkflowExecution>();
        services.AddLinKitDependency();

        return services;
    }
}
