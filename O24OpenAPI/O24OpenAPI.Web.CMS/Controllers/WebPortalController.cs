using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Services;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers;

/// <summary>
/// The web portal controller class
/// </summary>
/// <seealso cref="BaseController"/>

public class WebPortalController : BaseController
{
    /// <summary>
    /// The web portal service
    /// </summary>
    private readonly IWebPortalService _webPortalService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebPortalController"/> class
    /// </summary>
    /// <param name="webPortalService">The web portal service</param>
    public WebPortalController(IWebPortalService webPortalService)
    {
        _webPortalService = webPortalService;
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    [HttpPost]
    public virtual async Task<IActionResult> Post(WebPortalRequest request)
    {
        var result = await _webPortalService.Process(request);
        return Ok(result);
    }
}
