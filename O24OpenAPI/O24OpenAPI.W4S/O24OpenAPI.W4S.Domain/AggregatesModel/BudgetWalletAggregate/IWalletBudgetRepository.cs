using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletBudgetRepository : IRepository<WalletBudget>
{
    Task<List<WalletBudget>> GetByWalletIdsAsync(List<string> walletIds);
}
