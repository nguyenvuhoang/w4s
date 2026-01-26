using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Abstractions;
using OpenAI.Chat;
using OpenAI.Embeddings;

namespace O24OpenAPI.AI.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var configuration = EngineContext.Current.Resolve<LLMProviderConfig>();
        services.AddLinKitCqrs("ai");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            "ai"
        );
        services.AddLinKitDependency();
        services.AddSingleton(new EmbeddingClient(configuration.OpenAI.EmbedModel, configuration.OpenAI.ApiKey));
        services.AddSingleton(new ChatClient(configuration.OpenAI.ChatModel, configuration.OpenAI.ApiKey));
        return services;
    }
}
