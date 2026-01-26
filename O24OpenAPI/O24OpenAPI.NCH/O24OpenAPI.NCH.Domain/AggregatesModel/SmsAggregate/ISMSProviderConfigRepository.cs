using System;
using System.Collections.Generic;
using System.Text;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate
{
    public interface ISMSProviderConfigRepository : IRepository<SMSProviderConfig>
    {
        Task<string> BuildSOAPRequestAsync(SMSProvider provider, Dictionary<string, string> values);
        Task<string> GetConfigValueAsync(string providerName, string key);
        Task<string> AddDynamicSoapHeaders(StringContent content, string providerName);
    }
}
