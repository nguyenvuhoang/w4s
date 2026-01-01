using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.W4S.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs("w4s");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(serviceKey: "w4s");
        return services;
    }
}
