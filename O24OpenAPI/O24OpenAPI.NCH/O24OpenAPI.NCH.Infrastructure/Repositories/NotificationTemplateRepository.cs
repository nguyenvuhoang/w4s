using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class NotificationTemplateRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<NotificationTemplate>(dataProvider, staticCacheManager),
        INotificationTemplateRepository
{
    public Task<NotificationTemplate?> GetByCodeAsync(string code) =>
        throw new NotImplementedException();

    public async Task<NotificationTemplate> GetByTemplateIdAsync(string templateId)
    {
        return await Table.FirstOrDefaultAsync(x => x.TemplateID == templateId);
    }
}
