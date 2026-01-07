using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletCategoryRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletCategory>(dataProvider, staticCacheManager),
        IWalletCategoryRepository
{
    public async Task BulkInsertAsync(IList<WalletCategory> items)
    {
        await BulkInsert(items);
    }

    /// <summary>
    /// Exists the specified wallet identifier and category identifier.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    public async Task<bool> ExistsAsync(int walletId, string categorycode)
    {
        return await Table.AnyAsync(wc => wc.WalletId == walletId && wc.CategoryCode == categorycode);
    }

    public async Task<List<WalletCategory>> GetByWalletIdsAsync(List<int> walletIds)
    {
        if (walletIds == null || walletIds.Count == 0)
            return [];

        return await Table
            .Where(x => walletIds.Contains(x.WalletId))
            .ToListAsync();
    }
}
