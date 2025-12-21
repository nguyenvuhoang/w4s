using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Events;
using O24OpenAPI.Data;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserBannerRepository(
    IEventPublisher eventPublisher,
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<UserBanner>(eventPublisher, dataProvider, staticCacheManager),
        IUserBannerRepository
{

public async Task<UserBanner?> GetByUserCodeAsync(string userCode)
{
    return await Table.Where(x => x.UserCode == userCode).FirstOrDefaultAsync();
}

public async Task<string?> GetBannerSourceAsync(string userCode)
{
    return await Table.Where(x => x.UserCode == userCode).Select(x => x.BannerSource).FirstOrDefaultAsync();
}
}
