using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.PMT.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs();
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            "pmt"
        );
        return services;
    }
}
