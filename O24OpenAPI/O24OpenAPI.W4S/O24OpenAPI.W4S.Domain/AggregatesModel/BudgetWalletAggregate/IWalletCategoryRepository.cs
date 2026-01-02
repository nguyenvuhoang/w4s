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
    Task<bool> ExistsAsync(string walletId, string categoryId);
    /// <summary>
    /// Bulks the insert asynchronous.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    Task BulkInsertAsync(IList<WalletCategory> items);

}
