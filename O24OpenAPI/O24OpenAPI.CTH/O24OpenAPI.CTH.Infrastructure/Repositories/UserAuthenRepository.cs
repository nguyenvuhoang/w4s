using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserAuthenRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserAuthen>(eventPublisher, dataProvider, staticCacheManager),
        IUserAuthenRepository
{

public async Task<UserAuthen?> GetByUserCodeAsync(string userCode)
{
    return await Table.Where(s => s.UserCode == userCode).FirstOrDefaultAsync();
}

public async Task<UserAuthen?> GetByUserAuthenInfoAsync(string userCode, string authenType, string phoneNumber)
{
    return await Table.Where(s =>
            s.UserCode == userCode
            && s.AuthenType == authenType
            && s.Phone == phoneNumber
        )
        .FirstOrDefaultAsync();
}
}
