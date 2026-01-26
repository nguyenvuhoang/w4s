using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletProfileRepository : IRepository<WalletProfile>
{
    Task<WalletProfile> AddAsync(WalletProfile profile);
    Task<bool> ExistsByWalletIdAsync(int walletId);
    Task<WalletProfile> GetByWalletIdAsync(int walletId);
    Task<List<WalletProfile>> GetByContractNumber(string Contractnumber);
    Task<WalletProfile> GetByContractNumberAsync(string contractNumber);
}
