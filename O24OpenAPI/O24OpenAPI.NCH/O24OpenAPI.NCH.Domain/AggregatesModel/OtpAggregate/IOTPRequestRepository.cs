using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;

public interface IOTPRequestRepository : IRepository<OTP_REQUESTS>
{
    Task<OTP_REQUESTS?> GetLatestByPhoneAsync(string phoneNumber);
}
