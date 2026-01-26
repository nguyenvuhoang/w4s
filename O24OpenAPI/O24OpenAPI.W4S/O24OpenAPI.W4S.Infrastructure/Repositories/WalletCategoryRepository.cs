using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using System.Linq.Dynamic.Core;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletCategoryRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletCategory>(dataProvider, staticCacheManager), IWalletCategoryRepository
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
        return await Table.AnyAsync(wc =>
            wc.WalletId == walletId && wc.CategoryCode == categorycode
        );
    }

    /// <summary>
    /// Exist by wallet group and name asynchronous.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="categoryGroup"></param>
    /// <param name="categoryName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> ExistsByWalletGroupAndNameAsync(int walletId, string categoryGroup, string categoryName, CancellationToken cancellationToken = default)
    {
        var existing = await Table.Where(wc =>
            wc.WalletId == walletId &&
            wc.CategoryGroup == categoryGroup &&
            wc.CategoryName == categoryName
        ).FirstOrDefaultAsync(token: cancellationToken);

        return existing != null;
    }

    public async Task<List<WalletCategory>> GetByWalletIdsAsync(List<int> walletIds)
    {
        if (walletIds == null || walletIds.Count == 0)
            return [];

        return await Table.Where(x => walletIds.Contains(x.WalletId)).ToListAsync();
    }

    public async Task<List<WalletCategory>> GetWalletCategoryByWalletIdAsync(int walletid)
    {
        if (walletid <= 0)
            return [];

        return await Table.Where(x => x.WalletId == walletid).ToListAsync();
    }
}
