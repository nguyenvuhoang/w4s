using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class GroupUserNotificationRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<GroupUserNotification>(dataProvider, staticCacheManager),
        IGroupUserNotificationRepository
{
    public Task<IReadOnlyList<GroupUserNotification>> GetByGroupAsync(long groupId) =>
        throw new NotImplementedException();
}
