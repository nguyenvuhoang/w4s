using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserLimitRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserLimit>(eventPublisher, dataProvider, staticCacheManager),
        IUserLimitRepository
{

    public async Task<UserLimit> GetUserLimitToUpdate(int roleId, string commandId, string currencyCode, string limitType )
    {
        var uLimit = await 
            Table.Where(s =>
                s.RoleId == roleId
                && s.CommandId == commandId
                && s.CurrencyCode == currencyCode
                && s.LimitType == limitType
            )
            .FirstOrDefaultAsync();
        return uLimit;
    }
}
