using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

public class MailConfigRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<MailConfig>(dataProvider, staticCacheManager), IMailConfigRepository
{
    public Task<MailConfig?> GetActiveAsync() => throw new NotImplementedException();

    public virtual async Task<MailConfig> GetByConfigId(string configId)
    {
        return await Table.Where(s => s.ConfigId.Equals(configId)).FirstOrDefaultAsync();
    }
}
