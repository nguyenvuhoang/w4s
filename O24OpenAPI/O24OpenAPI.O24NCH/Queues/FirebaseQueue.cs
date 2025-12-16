using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Queues;

public class FirebaseQueue : BaseQueue
{
    private readonly IFirebaseService _firebaseService =
        EngineContext.Current.Resolve<IFirebaseService>();

    /// <summary>
    /// Generates a Firebase notification.
    /// </summary>
    /// <param name="wfScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> GenereateFirebaseNotification(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<FirebaseNotificationRequestModel>();
        return await Invoke<FirebaseNotificationRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _firebaseService.GenereateFirebaseNotificationAsync(model);
                return response;
            }
        );
    }
}
