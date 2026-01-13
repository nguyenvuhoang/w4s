using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class CurrencyRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<Currency>(dataProvider, staticCacheManager),
        ICurrencyRepository
{
    public async Task<List<Currency>> GetAllCurrenciesAsync(CancellationToken ct = default)
    {
        return await Table.ToListAsync(ct);
    }

    public async Task<string> GetShortCurrencyIdAsync(string currencyCode, CancellationToken ct = default)
    {
        return currencyCode == null
            ? throw new ArgumentNullException(nameof(currencyCode))
            : await Table
                .Where(c => c.CurrencyId == currencyCode)
                .Select(c => c.ShortCurrencyId)
                .FirstOrDefaultAsync(ct);
    }
}
