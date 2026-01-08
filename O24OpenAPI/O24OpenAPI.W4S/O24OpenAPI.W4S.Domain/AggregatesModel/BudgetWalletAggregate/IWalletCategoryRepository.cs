using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletCategoryRepository : IRepository<WalletCategory>
{
    /// <summary>
    /// Exists the specified wallet identifier.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(int walletId, string categorycode);

    /// <summary>
    /// Bulks the insert asynchronous.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    Task BulkInsertAsync(IList<WalletCategory> items);

    /// <summary>
    /// Get by wallet ids asynchronous.
    /// </summary>
    /// <param name="walletIds"></param>
    /// <returns></returns>
    Task<List<WalletCategory>> GetByWalletIdsAsync(List<int> walletIds);
}
