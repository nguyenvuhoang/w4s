using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public interface INotificationTemplateRepository : IRepository<NotificationTemplate>
{
    Task<NotificationTemplate?> GetByTemplateIdAsync(string templateId);
    Task<IReadOnlyList<NotificationTemplate>> GetByTemplateIdsAsync(
        IEnumerable<string> templateIds
    );
}
