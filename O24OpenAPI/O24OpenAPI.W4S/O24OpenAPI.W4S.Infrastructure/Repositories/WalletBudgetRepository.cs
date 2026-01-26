using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletBudgetRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletBudget>(dataProvider, staticCacheManager), IWalletBudgetRepository
{
    public async Task<List<WalletBudget>> GetByWalletIdsAsync(List<int> walletIds)
    {
        if (walletIds == null || walletIds.Count == 0)
            return [];

        return await Table.Where(x => walletIds.Contains(x.WalletId)).ToListAsync();
    }

    public async Task<List<WalletBudget>> GetByCategoryIdAndWalletIdAsync(
        int categoryId,
        int walletId
    )
    {
        return await Table
            .Where(x => x.CategoryId == categoryId && x.WalletId == walletId)
            .ToListAsync();
    }
}
