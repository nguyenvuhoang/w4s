using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers;

public class UserPortalController : BaseController
{

    /// <summary>
    /// The web portal service
    /// </summary>
    private readonly IUserPortalService __userPortalService;



    /// <summary>
    ///
    /// </summary>
    /// <param name="userPortalService"></param>
    public UserPortalController(IUserPortalService userPortalService)
    {
        __userPortalService = userPortalService;
    }

    /// <summary>
    /// Posts the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the action result</returns>
    ///

    [HttpPost]
    public virtual async Task<ActionResult> GetAll()
    {
        var result = await __userPortalService.GetAll();
        return Ok(result);
    }



}
