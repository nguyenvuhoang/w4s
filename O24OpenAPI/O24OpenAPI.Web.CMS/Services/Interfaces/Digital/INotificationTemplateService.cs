namespace O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

public interface INotificationTemplateService
{
    Task<D_NOTIFICATION_TEMPLATE> GetTemplate(string templateID);
}
