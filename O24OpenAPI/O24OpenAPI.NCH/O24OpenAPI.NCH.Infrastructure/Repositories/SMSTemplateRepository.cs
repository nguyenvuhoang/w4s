using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class SMSTemplateRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<SMSTemplate>(dataProvider, staticCacheManager), ISMSTemplateRepository
{
    public Task<SMSTemplate?> GetByCodeAsync(string code) => throw new NotImplementedException();

    public async Task<string> BuildSMSContentDynamicAsync(
        string templateCode,
        Dictionary<string, object> values,
        string message = ""
    )
    {
        SMSTemplate? template = await Table.FirstOrDefaultAsync(t =>
            t.TemplateCode == templateCode && t.IsActive
        );

        if (template == null)
        {
            return message;
        }

        string content = template.MessageContent;

        foreach (KeyValuePair<string, object> kvp in values)
        {
            content = content.Replace($"{{{kvp.Key}}}", kvp.Value?.ToString());
        }

        return content;
    }

    public Task<IReadOnlyList<SMSTemplate>> GetActiveAsync()
    {
        throw new NotImplementedException();
    }
}
