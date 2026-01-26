using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletCategoryGLsRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletCatalogGLs>(dataProvider, staticCacheManager),
        IWalletCategoryGLsRepository
{
    public async Task<WalletCatalogGLs> GetAccountingAccount(string catalogCode, string systaccountname)
    {
        return Table.Where(x => x.CatalogCode == catalogCode && x.SysAccountName == systaccountname)
            .FirstOrDefault();
    }
}
