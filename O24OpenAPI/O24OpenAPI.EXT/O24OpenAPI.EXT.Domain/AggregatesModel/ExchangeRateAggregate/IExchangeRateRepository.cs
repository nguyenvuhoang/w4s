using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;

public interface IExchangeRateRepository : IRepository<ExchangeRate>
{
    Task<ExchangeRate?> GetByRateDateAndCurrencyAsync(
       DateTime rateDateUtc,
       string currencyCode,
       CancellationToken cancellationToken = default
   );
}
