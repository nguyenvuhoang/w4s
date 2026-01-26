using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Infrastructure.Extensions;
using OpenAI.Chat;
using OpenAI.Embeddings;
using Qdrant.Client;

namespace O24OpenAPI.AI.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {

        services.AddLinKitDependency();
        QdrantSettingConfig? qdrantSettingConfig =
            Singleton<AppSettings>.Instance?.Get<QdrantSettingConfig>();
        if (qdrantSettingConfig is not null)
            services.AddSingleton(_ =>
            {
                return new QdrantClient(
                    host: qdrantSettingConfig.Host,
                    port: qdrantSettingConfig.Port
                );
            });

        var configuration = Singleton<AppSettings>.Instance?.Get<LLMProviderConfig>();
        var openAi = configuration?.OpenAI;

        if (!string.IsNullOrWhiteSpace(openAi?.ApiKey)
            && !string.IsNullOrWhiteSpace(openAi?.ChatModel)
            && !string.IsNullOrWhiteSpace(openAi?.EmbedModel))
        {
            services.AddSingleton(_ =>
                new EmbeddingClient(openAi.EmbedModel, openAi.ApiKey)
            );

            services.AddSingleton(_ =>
                new ChatClient(openAi.ChatModel, openAi.ApiKey)
            );
        }

        return services;
    }

    public static async Task ConfigureInfrastructure(this IApplicationBuilder app)
    {
        app.ConfigureRequestPipeline();
        await app.StartEngine();
    }
}
