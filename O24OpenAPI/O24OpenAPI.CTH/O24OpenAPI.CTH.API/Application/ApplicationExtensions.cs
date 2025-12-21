using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.CTH.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs();
        services.AddSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>();
        return services;
    }
}
