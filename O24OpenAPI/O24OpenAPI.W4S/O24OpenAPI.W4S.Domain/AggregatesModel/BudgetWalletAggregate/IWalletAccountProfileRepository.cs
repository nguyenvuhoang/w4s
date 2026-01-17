using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletAccountProfileRepository : IRepository<WalletAccount>
{
    /// <summary>
    /// Get wallet account by wallet id
    /// </summary>
    /// <param name="walletId"></param>
    /// <returns></returns>
    Task<List<WalletAccount>> GetWalletAccountByWalletIdAsync(List<int> walletId);
    /// <summary>
    /// Create default account
    /// </summary>
    /// <returns></returns>
    Task<List<WalletAccount>> CreateDefaultAccount(int walletId, string currencycode);
    /// <summary>
    /// Get default expense account
    /// </summary>
    /// <param name="walletId"></param>
    /// <returns></returns>
    Task<WalletAccount> GetDefaultExpenseAccountAsync(int walletId);
}
