using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletBalanceRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletBalance>(dataProvider, staticCacheManager), IWalletBalanceRepository
{
    public async Task<List<WalletBalance>> GetByAccountNumbersAsync(List<string> accountNumbers)
    {
        if (accountNumbers == null || accountNumbers.Count == 0)
            return [];

        return await Table
            .Where(x => accountNumbers.Contains(x.AccountNumber))
            .ToListAsync();
    }
}
