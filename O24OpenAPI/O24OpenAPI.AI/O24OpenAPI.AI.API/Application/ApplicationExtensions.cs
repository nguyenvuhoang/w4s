using Microsoft.Extensions.AI;
using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Framework.Abstractions;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;
using System.Text.Json;

namespace O24OpenAPI.AI.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });
        builder.Configuration.AddJsonFile(
            "StaticConfig/LLMProviderConfig.json",
            optional: true,
            reloadOnChange: true
        );

        LLMProviderConfig llmConfig = new();
        builder.Configuration.GetSection("LLMProviderConfig").Bind(llmConfig);
        builder.Services.AddSingleton(llmConfig);

        services.AddLinKitCqrs("ai");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(
            "ai"
        );
        services.AddLinKitDependency();
        builder.Services.AddSingleton(sp =>
        {
            LLMProviderConfig config = sp.GetRequiredService<LLMProviderConfig>();

            return new OpenAIClient(config.OpenAI.ApiKey!)
                .GetChatClient(config.OpenAI.ChatModel!)
                .AsIChatClient();
        });

        OpenAIConfig openAi = llmConfig.OpenAI;

        if (
            !string.IsNullOrWhiteSpace(openAi?.ApiKey)
            && !string.IsNullOrWhiteSpace(openAi?.ChatModel)
            && !string.IsNullOrWhiteSpace(openAi?.EmbedModel)
        )
        {
            services.AddSingleton(_ => new EmbeddingClient(openAi.EmbedModel, openAi.ApiKey));

            services.AddSingleton(_ => new ChatClient(openAi.ChatModel, openAi.ApiKey));
        }

        return services;
    }
}
