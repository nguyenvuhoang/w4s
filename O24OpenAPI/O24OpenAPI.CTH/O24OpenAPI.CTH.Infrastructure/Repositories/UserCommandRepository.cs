using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserCommandRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserCommand>(eventPublisher, dataProvider, staticCacheManager),
        IUserCommandRepository
{
    public async Task<List<string>> GetListCommandParentAsync(string applicationCode)
    {
        return await
            Table.Where(s =>
                s.ApplicationCode == applicationCode
                && s.CommandType == "M"
                && s.Enabled
                && s.ParentId == "0"
            )
            .Select(s => s.CommandId)
            .Distinct()
            .ToListAsync();
    }
}
