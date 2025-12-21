using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserRightChannelRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserRightChannel>(eventPublisher, dataProvider, staticCacheManager),
        IUserRightChannelRepository
{

public async Task<List<UserRightChannel>> GetByRoleIdAsync(int roleId)
{
    return await Table.Where(x => x.RoleId == roleId).ToListAsync();
}
}
