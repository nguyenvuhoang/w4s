using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletGoalRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletGoal>(dataProvider, staticCacheManager), IWalletGoalRepository
{
    public async Task<List<WalletGoal>> GetByWalletIdsAsync(List<int> walletIds)
    {
        if (walletIds == null || walletIds.Count == 0)
            return [];

        return await Table
            .Where(x => walletIds.Contains(x.WalletId))
            .ToListAsync();
    }
}
