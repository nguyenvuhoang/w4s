using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.Web.CMS.Controllers;

public class SessionController(
    IStaticTokenService staticTokenService,
    IUserSessionsService userSessionService
) : BaseController
{
    private readonly IStaticTokenService _staticTokenService = staticTokenService;
    private readonly IUserSessionsService _userSessionService = userSessionService;

    [HttpPost]
    public virtual async Task<IActionResult> CreateStaticToken(string identifier)
    {
        var response = await _staticTokenService.CreateStaticToken(identifier);
        return Ok(response);
    }

    [HttpPost]
    public virtual async Task<IActionResult> RevokeStaticToken(string token, string reason)
    {
        var response = await _staticTokenService.RevokeStaticToken(token, reason);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> ClearUserSession(string loginName)
    {
        await _userSessionService.ClearSession(loginName);
        return Ok("Successful");
    }
}
