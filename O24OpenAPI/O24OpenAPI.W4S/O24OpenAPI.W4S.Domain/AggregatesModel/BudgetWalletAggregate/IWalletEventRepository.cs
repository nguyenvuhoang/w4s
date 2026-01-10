using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletEventRepository : IRepository<WalletEvent>
{
    Task<List<WalletEvent>> GetByWalletIdsAsync(int walletIds);
}
