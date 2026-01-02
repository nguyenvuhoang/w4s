using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.NCH.API.Application.Extensions;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs("nch");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            serviceKey: "nch"
        );
        services.AddLinKitDependency();
        return services;
    }
}
