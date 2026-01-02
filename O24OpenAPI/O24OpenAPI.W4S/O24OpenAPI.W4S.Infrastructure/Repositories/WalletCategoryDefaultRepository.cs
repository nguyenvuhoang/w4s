using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletCategoryDefaultRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletCategoryDefault>(dataProvider, staticCacheManager),
        IWalletCategoryDefaultRepository
{
    public async Task<IList<WalletCategoryDefault>> GetActiveAsync(string language = null)
    {
        var query = Table.Where(wcd => wcd.IsActive);

        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query.Where(wcd => wcd.Language == language);
        }

        return await query.ToListAsync();
    }

}
