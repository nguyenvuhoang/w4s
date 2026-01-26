using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.AI.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs("ai");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            "ai"
        );
        services.AddLinKitDependency();
        return services;
    }
}
