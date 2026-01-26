using LinKit.Core.Abstractions;
using O24OpenAPI.AI.Domain.AggregatesModel.ChatHistoryAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.AI.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class ChatHistoryRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ChatHistory>(dataProvider, staticCacheManager),
        Domain.AggregatesModel.ChatHistoryAggregate.IChatHistoryRepository
{ }
