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
    /// <summary>
    /// Get Wallet Category By Wallet Id Asynchronous.
    /// </summary>
    /// <param name="walletid"></param>
    /// <returns></returns>
    Task<List<WalletCategory>> GetWalletCategoryByWalletIdAsync(int walletid);
    /// <summary>
    /// Exist by wallet group and name asynchronous.
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="categoryGroup"></param>
    /// <param name="categoryName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ExistsByWalletGroupAndNameAsync(int walletId, string categoryGroup, string categoryName, CancellationToken cancellationToken = default);
}
