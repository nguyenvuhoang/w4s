using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.ACT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class AccountChartRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<AccountChart>(dataProvider, staticCacheManager), IAccountChartRepository
{
    public Task<AccountChart?> GetByAccountNumberAsync(string accountNumber) =>
        Table.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

    public async Task<IReadOnlyList<AccountChart>> GetByBranchCodeCurrencyCodeAsync(
        string branchCode,
        string currencyCode
    ) =>
        await Table
            .Where(x => x.BranchCode == branchCode && x.CurrencyCode == currencyCode)
            .ToListAsync();

    public virtual bool IsAccountNumberExist(string acno)
    {
        AccountChart? accountchart = Table
            .Where(c => c.AccountNumber.Equals(acno))
            .FirstOrDefault();
        return accountchart != null;
    }
}
