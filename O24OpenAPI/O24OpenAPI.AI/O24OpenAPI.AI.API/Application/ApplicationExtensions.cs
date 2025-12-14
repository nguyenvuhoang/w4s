using O24OpenAPI.Web.Framework.Abstractions;

namespace O24OpenAPI.AI.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs();
        services.AddSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>();
        return services;
    }
}
