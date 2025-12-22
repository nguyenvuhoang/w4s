using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserRightRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserRight>(eventPublisher, dataProvider, staticCacheManager),
        IUserRightRepository
{
    public async Task<List<string>> GetExistingCommandIdsAsync(
        int roleId,
        ICollection<string> commandIds
    )
    {
        if (commandIds == null || commandIds.Count == 0)
        {
            return [];
        }

        return await Table
            .Where(r => r.RoleId == roleId && commandIds.Contains(r.CommandId))
            .Select(r => r.CommandId)
            .Distinct()
            .ToListAsync();
    }


}
