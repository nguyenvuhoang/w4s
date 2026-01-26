using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.API.Application.Utils;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;
using System.Text;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SMSProviderConfigRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<SMSProviderConfig>(dataProvider, staticCacheManager),
        ISMSProviderConfigRepository
{
    public async Task<string> BuildSOAPRequestAsync(
        SMSProvider provider,
        Dictionary<string, string> values
    )
    {
        List<SMSProviderConfig> configs = await Table
            .Where(x => x.SMSProviderId == provider.ProviderName && x.IsActive)
            .ToListAsync();

        var configDict = configs.ToDictionary(x => x.ConfigKey, x => x.ConfigValue);

        if (!configDict.TryGetValue("SOAP_TEMPLATE", out var template))
        {
            throw new Exception("SOAP_TEMPLATE not found.");
        }

        switch (provider.ProviderName?.ToUpperInvariant())
        {
            case "UNITEL":
                template = template.Replace("{USERNAME}", provider.ApiUsername ?? string.Empty);
                template = template.Replace("{PASSWORD}", provider.ApiPassword ?? string.Empty);
                template = template.Replace("{BRAND}", provider.BrandName ?? string.Empty);
                break;

            case "LTC":
                values.TryGetValue("RECEIVERPHONE", out var receiverPhone);
                values.TryGetValue("TRANSACTIONID", out var transactionId);
                var dynamicKey = $"{provider.ApiUsername}{transactionId}{receiverPhone}";
                string genkey = LTC.Encrypt(dynamicKey, provider.ApiKey);
                template = template.Replace("{LTCKEY}", genkey ?? string.Empty);
                template = template.Replace("{LTCUSER}", provider.ApiUsername ?? string.Empty);
                break;
            case "ETL":
                template = template.Replace("{USERNAME}", provider.ApiUsername ?? string.Empty);

                values.TryGetValue("RECEIVERPHONE", out var eltReceiverPhone);
                values.TryGetValue("TRANSACTIONID", out var etlTransactionId);
                var sign = Etl.GenerateSign(
                    spId: provider.ApiUsername,
                    transactionId: etlTransactionId,
                    msisdn: eltReceiverPhone,
                    key: provider.ApiKey,
                    url: "https://manage.etllao.com"
                );
                template = template.Replace(
                    "{ETLSIGN}",
                    sign?.ToUpperInvariant() ?? string.Empty
                );
                break;
            default:
                // fallback: replace common keys if present
                template = template.Replace("{USERNAME}", provider.ApiUsername ?? string.Empty);
                template = template.Replace("{PASSWORD}", provider.ApiPassword ?? string.Empty);
                template = template.Replace("{BRAND}", provider.BrandName ?? string.Empty);
                break;
        }

        foreach (KeyValuePair<string, string> pair in values)
        {
            template = template.Replace($"{{{pair.Key}}}", pair.Value ?? string.Empty);
        }

        return template;
    }

    public async Task<string> GetConfigValueAsync(string providerName, string key)
    {
        return await Table
            .Where(x => x.SMSProviderId == providerName && x.ConfigKey == key && x.IsActive)
            .Select(x => x.ConfigValue)
            .FirstOrDefaultAsync();
    }

    public async Task<string> AddDynamicSoapHeaders(StringContent content, string providerName)
    {
        List<SMSProviderConfig> headerConfigs = await Table
            .Where(x =>
                x.SMSProviderId == providerName
                && x.ConfigKey.StartsWith("SOAP_HEADER_")
                && x.IsActive
            )
            .ToListAsync();

        var headerLog = new StringBuilder();
        foreach (SMSProviderConfig? header in headerConfigs)
        {
            var headerKey = header.ConfigKey["SOAP_HEADER_".Length..];

            content.Headers.Add(headerKey, header.ConfigValue);
            headerLog.AppendLine($"{headerKey}: {header.ConfigValue}");
        }

        return headerLog.ToString().Trim();
    }
}
