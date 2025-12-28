using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

public interface ISMSProviderRepository : IRepository<SMSProvider>
{
    Task<SMSProvider?> GetByCodeAsync(string providerCode);
    Task<IReadOnlyList<SMSProvider>> GetActiveAsync();
}
