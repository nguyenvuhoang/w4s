using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;

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
                var response = await _userNotificationsService.CreateUserNotifications(
                    model.UserCode,
                    model.Category
                );
                return response;
            }
        );
    }
}
