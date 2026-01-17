using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletAccountGLsRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletAccountGLs>(dataProvider, staticCacheManager),
        IWalletAccountGLsRepository
{
    public async Task<WalletAccountGLs> GetAccountingAccount(string accountnumber, string accounttype)
    {
        return await Table
                   .Where(x => x.WalletAccount == accountnumber && x.SysAccountName == accounttype)
                   .FirstOrDefaultAsync();
    }
}
