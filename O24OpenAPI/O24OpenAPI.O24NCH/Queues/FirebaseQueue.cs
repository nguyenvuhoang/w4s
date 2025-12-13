using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.O24NCH.Queues;

public class FirebaseQueue : BaseQueue
{
    private readonly IFirebaseService _firebaseService = EngineContext.Current.Resolve<IFirebaseService>();


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
