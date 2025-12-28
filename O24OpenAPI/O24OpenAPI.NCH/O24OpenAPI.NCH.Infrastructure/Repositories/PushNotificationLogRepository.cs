using System;
using System.Collections.Generic;
using System.Text;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories
{
    public class PushNotificationLogRepository(
        IO24OpenAPIDataProvider dataProvider,
        IStaticCacheManager staticCacheManager
    )
        : EntityRepository<PushNotificationLog>(dataProvider, staticCacheManager),
            IPushNotificationLogRepository
    {
        public Task<IReadOnlyList<PushNotificationLog>> GetByUserAsync(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
