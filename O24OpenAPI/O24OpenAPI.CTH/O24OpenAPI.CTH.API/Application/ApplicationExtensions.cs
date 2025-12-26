using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.CTH.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs("cth");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>("cth");
        return services;
    }
}
