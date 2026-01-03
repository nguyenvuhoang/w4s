using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.ACT.API.Application.Extensions;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddACTApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs("cth");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            serviceKey: "cth"
        );
        return services;
    }
}
