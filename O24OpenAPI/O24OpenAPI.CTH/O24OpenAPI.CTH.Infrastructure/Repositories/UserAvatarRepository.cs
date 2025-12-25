using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserAvatarRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserAvatar>(dataProvider, staticCacheManager), IUserAvatarRepository
{
    public async Task<UserAvatar> GetByUserCodeAsync(string userCode)
    {
        return await Table.Where(s => s.UserCode == userCode).FirstOrDefaultAsync();
    }
}
