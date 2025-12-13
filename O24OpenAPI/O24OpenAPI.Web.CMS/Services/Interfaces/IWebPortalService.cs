using O24OpenAPI.Web.CMS.Services.Services;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

/// <summary>
/// The web portal service interface
/// </summary>

public interface IWebPortalService
{
    /// <summary>
    /// Processes the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the web portal response</returns>
    Task<WebPortalResponse> Process(WebPortalRequest request);

}