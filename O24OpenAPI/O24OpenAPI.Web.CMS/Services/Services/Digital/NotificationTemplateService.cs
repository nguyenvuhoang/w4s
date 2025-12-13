using LinqToDB;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class NotificationTemplateService : INotificationTemplateService
{
    private readonly IRepository<D_NOTIFICATION_TEMPLATE> _notificationTemplateRepository;

    public NotificationTemplateService(
        IRepository<D_NOTIFICATION_TEMPLATE> notificationTemplateRepository
    )
    {
        _notificationTemplateRepository = notificationTemplateRepository;
    }

    public async Task<D_NOTIFICATION_TEMPLATE> GetTemplate(string templateID)
    {
        return await _notificationTemplateRepository
            .Table.Where(x => x.TemplateID == templateID)
            .FirstOrDefaultAsync();
    }
}
