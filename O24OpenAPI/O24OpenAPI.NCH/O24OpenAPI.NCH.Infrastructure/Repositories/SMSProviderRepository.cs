using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SMSProviderRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SMSProvider>(dataProvider, staticCacheManager), ISMSProviderRepository
{
    public Task<SMSProvider?> GetByCodeAsync(string providerCode) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<SMSProvider>> GetActiveAsync() => throw new NotImplementedException();

    public async Task<SMSProvider> GetProviderByPhoneNumber(string phoneNumber)
    {
        List<SMSProvider> providers = await Table.Where(p => p.IsActive).ToListAsync();
        foreach (SMSProvider? provider in providers)
        {
            List<string> allowedPrefixes =
                provider
                    .AllowedPrefix?.Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                    .ToList()
                ?? [];
            foreach (var prefix in allowedPrefixes)
            {
                if (phoneNumber.StartsWith(prefix))
                {
                    return provider;
                }
            }
        }
        return null;
    }
}
