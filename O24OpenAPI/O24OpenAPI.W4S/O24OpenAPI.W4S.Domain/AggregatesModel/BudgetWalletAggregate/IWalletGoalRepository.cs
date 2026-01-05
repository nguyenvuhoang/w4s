using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletGoalRepository : IRepository<WalletGoal>
{
    Task<List<WalletGoal>> GetByWalletIdsAsync(List<string> walletIds);
}
