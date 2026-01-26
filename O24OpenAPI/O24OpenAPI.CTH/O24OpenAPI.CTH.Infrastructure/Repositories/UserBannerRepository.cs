using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.CTH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class UserBannerRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<UserBanner>(dataProvider, staticCacheManager), IUserBannerRepository
{
    public async Task<UserBanner?> GetByUserCodeAsync(string userCode)
    {
        return await Table.Where(x => x.UserCode == userCode).FirstOrDefaultAsync();
    }

    public async Task<string?> GetBannerSourceAsync(string userCode)
    {
        return await Table
            .Where(x => x.UserCode == userCode)
            .Select(x => x.BannerSource)
            .FirstOrDefaultAsync();
    }

    public async Task<string> GetUserBannerAsync(string usercode)
    {
        try
        {
            return Table
                    .Where(x => x.UserCode == usercode)
                    .Select(x => x.BannerSource)
                    .FirstOrDefault() ?? "default";
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return string.Empty;
        }
    }
}
