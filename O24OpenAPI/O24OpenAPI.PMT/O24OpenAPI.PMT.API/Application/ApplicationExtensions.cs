using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.PMT.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs(MediatorKey.PMT);
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            MediatorKey.PMT
        );
        return services;
    }
}
