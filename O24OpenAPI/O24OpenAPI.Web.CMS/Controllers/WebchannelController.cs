using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Models.Neptune;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.NeptuneService;
using O24OpenAPI.Web.Framework.Controllers;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Controllers;

public partial class WebchannelController(
    IWebChannelService webChannelService,
    INeptuneCBSService neptuneCBSService,
    JWebUIObjectContextModel context
) : BaseController
{
    #region Fields

    readonly IWebChannelService _webChannelService = webChannelService;
    private readonly JWebUIObjectContextModel _context = context;
    private readonly INeptuneCBSService _neptuneCBSService = neptuneCBSService;
    #endregion

    /// <summary>
    /// Post
    /// </summary>
    /// <param name="bo">.</param>
    /// <returns>IActionResult.</returns>
    ///
    [HttpPost]
    public virtual async Task<IActionResult> Post([FromBody] BoRequestModel bo)
    {
        try
        {
            Dictionary<string, string> getAllCookies = Utils.Utils.GetCookies(HttpContext);
            bool checkedSession = false;
            if (!getAllCookies.TryGetValue("device_id", out string value))
            {
                checkedSession = true;
            }
            else
            {
                var oldCookie = value.ToString();
                if (oldCookie != _context.InfoRequest.DeviceID)
                {
                    checkedSession = true;
                }
            }

            if (checkedSession && !string.IsNullOrEmpty(_context.InfoRequest.DeviceID))
            {
                HttpContext.Response.Cookies.Append(
                    "device_id",
                    _context.InfoRequest.DeviceID,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true,
                        Expires = EngineContext
                            .Current.Resolve<JWebUIObjectContextModel>()
                            .InfoRequest.SessionExpired,
                    }
                );
            }

            var response = await _webChannelService.StartRequest(bo, HttpContext);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var actionResponse = new ActionsResponseModel<object>();
            List<ErrorInfoModel> listError =
            [
                Utils.Utils.AddActionError(
                    ErrorType.errorSystem,
                    ErrorMainForm.danger,
                    ex.Message,
                    "ERROR",
                    "#ERROR_SYSTEM: "
                ),
            ];
            actionResponse.error.AddRange(listError);
            return Ok(actionResponse);
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> ExecuteWorkflow(
        [FromBody] ExecuteWorkflowNeptuneModel model
    )
    {
        var header = Utils.Utils.GetHeaders(HttpContext);
        if (!header.TryGetValue("uid", out string token))
        {
            return Unauthorized(new { message = "Token not found", errorCode = "AUTH_001" });
        }

        var tokenService = EngineContext.Current.Resolve<IStaticTokenService>();
        var (isValid, message) = tokenService.ValidateStaticToken(token);
        if (!isValid)
        {
            return StatusCode(401, new { message, errorCode = "AUTH_002" });
        }

        var response = await _neptuneCBSService.ExecuteWorkflow(model);
        return Ok(response);
    }

}
