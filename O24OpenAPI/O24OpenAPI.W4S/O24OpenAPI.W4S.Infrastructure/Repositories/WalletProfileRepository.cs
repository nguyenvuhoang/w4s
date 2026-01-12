using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletProfileRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletProfile>(dataProvider, staticCacheManager), IWalletProfileRepository
{
    public async Task<WalletProfile> AddAsync(WalletProfile profile)
    {
        return await InsertAsync(profile);
    }

    public async Task<bool> ExistsByWalletIdAsync(int walletId)
    {
        WalletProfile profile = await GetByWalletIdAsync(walletId);
        return profile != null;
    }

    public async Task<List<WalletProfile>> GetByContractNumber(string Contractnumber)
    {
        return await Table.Where(wp => wp.ContractNumber == Contractnumber).ToListAsync();
    }

    public Task<WalletProfile> GetByContractNumberAsync(string contractNumber)
    {
        var profile = Table.Where(wp => wp.ContractNumber == contractNumber).FirstOrDefaultAsync();
        return profile;
    }

    public async Task<WalletProfile> GetByWalletIdAsync(int walletId)
    {
        WalletProfile profile = await Table.Where(wp => wp.Id == walletId).FirstOrDefaultAsync();
        return profile;
    }
}
