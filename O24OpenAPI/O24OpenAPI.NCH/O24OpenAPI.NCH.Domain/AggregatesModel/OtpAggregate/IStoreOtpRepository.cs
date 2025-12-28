using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;

public interface IStoreOtpRepository : IRepository<StoreOtp>
{
    Task<StoreOtp?> GetLatestAsync(string phoneNumber, string reviewPlatform);
}
