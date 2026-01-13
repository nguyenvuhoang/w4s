using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate;

public interface ICurrencyRepository : IRepository<Currency>
{
    Task<string> GetShortCurrencyIdAsync(string currencyCode, CancellationToken ct = default);
    Task<List<Currency>> GetAllCurrenciesAsync(CancellationToken ct = default);
}
