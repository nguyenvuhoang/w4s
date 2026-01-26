using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;

public interface ICurrencyRepository : IRepository<Currency>
{
    Task<Currency?> GetByCodeAsync(string currencyCode);
}
