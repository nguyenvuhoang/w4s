using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

public interface INotificationTemplateRepository : IRepository<NotificationTemplate>
{
    Task<NotificationTemplate?> GetByCodeAsync(string code);
    Task<NotificationTemplate> GetByTemplateIdAsync(string templateId);
}
