using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserNotificationsRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserNotification>(dataProvider, staticCacheManager),
        IUserNotificationsRepository
{
    public Task<IReadOnlyList<UserNotification>> GetByUserAsync(long userId) =>
        throw new NotImplementedException();
}
