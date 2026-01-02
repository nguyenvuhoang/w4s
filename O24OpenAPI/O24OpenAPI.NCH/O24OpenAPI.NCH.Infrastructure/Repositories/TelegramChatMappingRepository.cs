using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

using O24OpenAPI.NCH.Domain.AggregatesModel.TelegramAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class TelegramChatMappingRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<TelegramChatMapping>(dataProvider, staticCacheManager), ITelegramChatMappingRepository
{
    public Task<TelegramChatMapping?> GetByChatIdAsync(string chatId) => throw new NotImplementedException();
}
