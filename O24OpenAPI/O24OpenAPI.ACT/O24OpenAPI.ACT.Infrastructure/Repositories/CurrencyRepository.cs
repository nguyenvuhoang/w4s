using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.ACT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class CurrencyRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<Currency>(dataProvider, staticCacheManager), ICurrencyRepository
{
    public async Task<Currency?> GetByCodeAsync(string currencyCode) =>
        await Table.FirstOrDefaultAsync(x =>
            x.CurrencyId == currencyCode || x.ShortCurrencyId == currencyCode
        );
}
