using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IWebChannelService
{
    ///<Summary>
    /// GetConfigClient
    ///</Summary>
    JObject GetConfigClient();

    /// <summary>
    ///
    /// </summary>
    /// <param name="bo"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    Task<ActionsResponseModel<object>> StartRequest(BoRequestModel bo, HttpContext httpContext);
}
