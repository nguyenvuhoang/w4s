using Newtonsoft.Json.Linq;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.O24NCH.Queues;

public class NotificationQueue(INotificationService notificationService) : BaseQueue
{
    private readonly INotificationService _notificationService = notificationService;

    public async Task<WFScheme> Search(WFScheme workflow)
    {
        var model = await workflow.ToModel<NotificationSearchModel>();

        return await Invoke<NotificationSearchModel>(
            workflow,
            async () =>
            {
                var response = await _notificationService.SearchAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> GetUnreadCount(WFScheme workflow)
    {
        var model = await workflow.ToModel<NotificationSearchModel>();
        return await Invoke<NotificationSearchModel>(
            workflow,
            async () =>
            {
                var userCode = model.UserCode ?? model.CurrentUserCode;
                var count = await _notificationService.GetUnreadCount(userCode, model.ChannelId);
                return new JObject { { "count", count } };
            }
        );
    }

    /// <summary>
    /// Sends a notification using the specified workflow scheme and returns the updated workflow scheme after the
    /// notification is processed.
    /// </summary>
    /// <param name="wfScheme">The workflow scheme containing the notification details to be sent. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated workflow scheme
    /// after the notification is sent.</returns>
    public async Task<WFScheme> SendNotification(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<NotificationRequestModel>();
        return await Invoke<NotificationRequestModel>(
            wfScheme,
        async () =>
        {
            var response = await _notificationService.SendNotification(model);
            return response;
        }
        );
    }

    public async Task<WFScheme> SendMobileDevice(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<SendMobileDeviceRequestModel>();
        return await Invoke<SendMobileDeviceRequestModel>(
            wfScheme,
        async () =>
        {
            var response = await _notificationService.SendMobileDeviceAsync(model);
            return response;
        });
    }
}
