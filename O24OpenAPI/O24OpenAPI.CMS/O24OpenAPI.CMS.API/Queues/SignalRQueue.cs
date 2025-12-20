using O24OpenAPI.CMS.API.Application.Models.Request;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.CMS.API.Queues;

public class SignalRQueue(ISignalHubBusinessService signalHubService) : BaseQueue
{
    private readonly ISignalHubBusinessService _signalHubService = signalHubService;

    public async Task<WFScheme> Send(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<SignalRSendModel>();
        return await Invoke<SignalRSendModel>(
            wFScheme,
            async () =>
            {
                var response = await _signalHubService.Send(model);
                return response;
            }
        );
    }
}
