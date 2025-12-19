using O24OpenAPI.CMS.API.Application.Models.Request;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface ISignalHubBusinessService
{
    /// <summary>
    /// Send SignalR Message
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Send(SignalRSendModel model);
}
