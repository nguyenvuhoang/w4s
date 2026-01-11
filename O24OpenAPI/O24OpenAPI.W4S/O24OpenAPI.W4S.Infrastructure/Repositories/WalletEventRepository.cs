using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletEventRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletEvent>(dataProvider, staticCacheManager), IWalletEventRepository
{
    public async Task<List<WalletEvent>> GetByWalletIdsAsync(int walletId)
    {
        if (walletId <= 0)
            return [];
        return await Table.Where(x => x.WalletId == walletId).ToListAsync();
    }
}
