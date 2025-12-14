using O24OpenAPI.Web.Framework.Abstractions;

namespace O24OpenAPI.AI.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs();
        services.AddSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>();
        services.AddKeyedScoped<LinKit.Core.Cqrs.IMediator, LinKit.Generated.Cqrs.Mediator>("AIService");
        return services;
    }
}
