using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletTransactionRepository : IRepository<WalletTransaction>
{
    Task<WalletTransaction> AddAsync(WalletTransaction transaction);
    Task<IList<WalletTransaction>> GetByWalletIdAsync(
        Guid walletId,
        DateTime? fromUtc = null,
        DateTime? toUtc = null,
        int skip = 0,
        int take = 50,
        CancellationToken cancellationToken = default
    );
}
