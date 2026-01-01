using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;
using O24OpenAPI.Logger.Infrastructure.Configurations;

namespace O24OpenAPI.Logger.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class ApplicationLogRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager,
    LoggerSetting loggerSetting
) : EntityRepository<ApplicationLog>(dataProvider, staticCacheManager), IApplicationLogRepository
{
    public Task ClearApplicationLogAsync()
    {
        return DeleteWhere(log =>
            log.LogTimestamp < DateTime.UtcNow.AddDays(-loggerSetting.LogRetentionDays)
        );
    }
}
