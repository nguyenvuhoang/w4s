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
)
    : EntityRepository<WalletEvent>(dataProvider, staticCacheManager),
        IWalletEventRepository
{
    public Task<List<WalletEvent>> GetByWalletIdsAsync(int walletIds)
    {
        if (walletIds <= 0)
            return Task.FromResult(new List<WalletEvent>());
        return Table
            .Where(x => x.WalletId == walletIds)
            .ToListAsync();
    }
}
