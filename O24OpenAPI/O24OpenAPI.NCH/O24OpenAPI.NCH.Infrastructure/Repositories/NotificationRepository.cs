using Newtonsoft.Json;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;

namespace O24OpenAPI.NCH.Infrastructure.Repositories;

public class NotificationRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<Notification>(dataProvider, staticCacheManager), INotificationRepository
{
    public Task<IReadOnlyList<Notification>> GetPendingAsync(int take) =>
        throw new NotImplementedException();

    public async Task<int> LogInformation(
        string userCode,
        string appType,
        string notificationType,
        string templateID,
        string redirect,
        string dataSending,
        string notificationCategory = "BALANCE",
        string message = "",
        string title = "",
        string imageUrl = ""
    )
    {
        try
        {
            var entity = new Notification
            {
                UserCode = userCode,
                AppType = appType,
                NotificationType = notificationType,
                TemplateID = templateID,
                Redirect = redirect,
                DataValue = JsonConvert.SerializeObject(dataSending),
                IsRead = false,
                IsPushed = false,
                DateTime = DateTime.Now,
                IsProcessed = false,
                NotificationCategory = notificationCategory,
                Message = message,
                Title = title,
                ImageUrl = imageUrl,
            };

            await Insert(entity);

            return entity.Id;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return -1;
        }
    }
}
