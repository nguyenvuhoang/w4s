using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.ACT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class AccountCommonRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<AccountCommon>(dataProvider, staticCacheManager), IAccountCommonRepository
{
    public Task<AccountCommon?> GetByAccountNumberAsync(string accountNumber) =>
        Table.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
}
