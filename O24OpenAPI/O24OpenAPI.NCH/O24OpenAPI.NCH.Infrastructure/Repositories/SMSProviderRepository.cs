using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

public class SMSProviderRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SMSProvider>(dataProvider, staticCacheManager), ISMSProviderRepository
{
    public Task<SMSProvider?> GetByCodeAsync(string providerCode) => throw new NotImplementedException();

    public Task<IReadOnlyList<SMSProvider>> GetActiveAsync() => throw new NotImplementedException();
}
