using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.O24NCH.Queues;

public class UserNotificationsQueue(IUserNotificationsService userNotificationsService) : BaseQueue
{
    private readonly IUserNotificationsService _userNotificationsService = userNotificationsService;
    /// <summary>
    /// Create User Notifications
    /// </summary>
    /// <param name="wfScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> CreateUserNotifications(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<UserNotificationsRequestModel>();
        return await Invoke<UserNotificationsRequestModel>(
            wfScheme,
        async () =>
        {
            var response = await _userNotificationsService.CreateUserNotifications(model.UserCode, model.Category);
            return response;
        });
    }
}
