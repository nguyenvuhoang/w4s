using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Web.CMS.Services.Services;
using O24OpenAPI.Web.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers
{
    public class NotificationController(
        INotificationService notificationService,
        INotificationTemplateService notificationTemplateService,
        IHubContext<SignalHubService> hubContext
    ) : BaseController
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly INotificationTemplateService _notificationTemplateService =
            notificationTemplateService;
        private readonly IHubContext<SignalHubService> _hubContext = hubContext;

        [HttpPost]
        public async Task<IActionResult> SendBulkPushNotifications(
            [FromBody] List<string> expoPushTokens,
            string title,
            string body
        )
        {
            try
            {
                await _notificationService.SendBulkPushNotificationsAsync(
                    expoPushTokens,
                    title,
                    body
                );
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            await _notificationService.SendNotificationAsync(
                request.Token,
                request.Title,
                request.Body,
                request.Data
            );

            return Ok("Notification sent successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody] NotificationSearchModel model)
        {
            var data = await _notificationService.Search(model);
            var list = new List<NotificationSearchResponse>();

            foreach (var item in data)
            {
                var obj = new NotificationSearchResponse(item);

                var template = await _notificationTemplateService.GetTemplate(item.TemplateID);
                string message = template.Body.GetMessage(obj.Data, model.Language);
                obj.Message = message;

                list.Add(obj);
            }
            var response = new NotificationSearchResponseModel(list, data);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendSignal(
            string channel,
            string userId,
            string deviceId,
            string message
        )
        {
            await SignalHubService.SignalSendToUser(
                _hubContext,
                channel,
                message,
                userId,
                deviceId
            );
            return Ok("oke");
        }

        [HttpPost]
        public async Task<IActionResult> GetUnreadCount(string userCode, string appType)
        {
            var count = await _notificationService.GetUnreadCount(userCode, appType);
            return Ok(count);
        }
    }
}

public class NotificationRequest
{
    public string Token { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Data { get; set; }
}
