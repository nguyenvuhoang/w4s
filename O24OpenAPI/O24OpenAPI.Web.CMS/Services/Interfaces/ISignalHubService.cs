using O24OpenAPI.Web.CMS.Models.Request;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ISignalHubBusinessService
{
    /// <summary>
    /// Send SignalR Message
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> Send(SignalRSendModel model);
}
