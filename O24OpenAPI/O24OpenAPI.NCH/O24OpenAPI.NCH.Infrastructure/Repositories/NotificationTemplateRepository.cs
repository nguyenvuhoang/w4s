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
    public async Task<NotificationTemplate?> GetByTemplateIdAsync(string templateId)
    {
        return await Table.FirstOrDefaultAsync(x => x.TemplateID == templateId);
    }

    public async Task<IReadOnlyList<NotificationTemplate>> GetByTemplateIdsAsync(
        IEnumerable<string> templateIds
    )
    {
        List<string> ids = templateIds?.Distinct().ToList() ?? [];
        if (ids.Count == 0)
        {
            return [];
        }

        return await Table.Where(x => ids.Contains(x.TemplateID)).ToListAsync();
    }
}
