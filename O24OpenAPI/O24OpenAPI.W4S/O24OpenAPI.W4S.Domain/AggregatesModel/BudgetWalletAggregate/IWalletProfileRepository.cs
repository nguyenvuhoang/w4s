using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletProfileRepository : IRepository<WalletProfile>
{
    Task<WalletProfile> AddAsync(WalletProfile profile);
    Task<bool> ExistsByWalletIdAsync(string WalletId);
    Task<WalletProfile> GetByWalletIdAsync(string WalletId);
    Task<List<WalletProfile>> GetByContractNumber(string Contractnumber);

}
