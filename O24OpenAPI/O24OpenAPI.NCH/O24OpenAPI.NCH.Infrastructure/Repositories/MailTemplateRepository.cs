using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

public class MailTemplateRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<MailTemplate>(dataProvider, staticCacheManager), IMailTemplateRepository
{
    public Task<MailTemplate?> GetByCodeAsync(string code) => throw new NotImplementedException();

    public virtual async Task<MailTemplate> GetByTemplateId(string templateId)
    {
        return await Table.Where(s => s.TemplateId.Equals(templateId)).FirstOrDefaultAsync();
    }
}
