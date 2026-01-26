using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

using O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class StoreOtpRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<StoreOtp>(dataProvider, staticCacheManager), IStoreOtpRepository
{
    public Task<StoreOtp?> GetLatestAsync(string phoneNumber, string reviewPlatform) => throw new NotImplementedException();
}
