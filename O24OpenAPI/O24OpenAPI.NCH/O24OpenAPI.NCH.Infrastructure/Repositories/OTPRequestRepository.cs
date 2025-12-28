using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

using O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

public class OTPRequestRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<OTP_REQUESTS>(dataProvider, staticCacheManager), IOTPRequestRepository
{
    public bool GetLatestByPhoneAsync(string phoneNumber) => throw new NotImplementedException();

    Task<OTP_REQUESTS?> IOTPRequestRepository.GetLatestByPhoneAsync(string phoneNumber)
    {
        throw new NotImplementedException();
    }
}
