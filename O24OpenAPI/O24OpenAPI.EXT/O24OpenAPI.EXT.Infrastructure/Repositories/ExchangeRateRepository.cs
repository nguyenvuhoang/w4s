using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;

namespace O24OpenAPI.EXT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ExchangeRateRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<ExchangeRate>(dataProvider, staticCacheManager), IExchangeRateRepository
{
    public async Task<ExchangeRate?> GetByRateDateAndCurrencyAsync(DateTime rateDateUtc, string currencyCode, CancellationToken cancellationToken = default)
    {
        return await Table.Where(
            er => er.RateDateUtc == rateDateUtc && er.CurrencyCode == currencyCode
        ).FirstOrDefaultAsync(cancellationToken);
    }
}
