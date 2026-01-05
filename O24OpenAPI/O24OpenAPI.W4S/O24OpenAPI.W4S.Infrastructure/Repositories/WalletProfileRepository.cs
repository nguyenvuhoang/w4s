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

    public async Task<bool> ExistsByWalletIdAsync(string WalletId)
    {
        var profile = await GetByWalletIdAsync(WalletId);
        return profile != null;
    }

    public async Task<List<WalletProfile>> GetByContractNumber(string Contractnumber)
    {
        return await Table.Where(wp => wp.ContractNumber == Contractnumber)
            .ToListAsync();
    }

    public async Task<WalletProfile> GetByWalletIdAsync(string WalletId)
    {
        var guidWalletId = Guid.Parse(WalletId);
        var profile = await Table.Where(wp => wp.WalletId == guidWalletId)
            .FirstOrDefaultAsync();
        return profile;
    }
}
