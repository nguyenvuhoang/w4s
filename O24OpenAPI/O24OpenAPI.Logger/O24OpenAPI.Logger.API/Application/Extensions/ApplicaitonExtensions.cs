namespace O24OpenAPI.Logger.API.Application.Extensions;

public static class ApplicaitonExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs("log");
        //services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
        //    "ai"
        //);
        return services;
    }
}
