using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface INotificationTemplateService
{
    Task<NotificationTemplate> GetByTemplateIdAsync(string templateId);
    Task<IReadOnlyList<NotificationTemplate>> GetByTemplateIdsAsync(IEnumerable<string> templateIds);

}
