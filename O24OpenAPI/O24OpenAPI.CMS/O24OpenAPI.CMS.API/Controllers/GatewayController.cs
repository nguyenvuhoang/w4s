using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.CMS.API.Application.Features.Requests;
using O24OpenAPI.CMS.API.Application.Models.ContextModels;
using O24OpenAPI.CMS.API.Application.Models.Response;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.CMS.API.Controllers;

public partial class GatewayController(
    IRequestHandler requestHandler,
    JWebUIObjectContextModel context
) : BaseController
{
    #region Fields

    private readonly JWebUIObjectContextModel _context = context;
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
            Dictionary<string, string> getAllCookies = Application.Utils.Utils.GetCookies(
                HttpContext
            );
            bool checkedSession = false;
            if (!getAllCookies.TryGetValue("device_id", out string? value))
            {
                checkedSession = true;
            }
            else
            {
                string oldCookie = value.ToString();
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
                            ?.InfoRequest.SessionExpired,
                    }
                );
            }

            ActionsResponseModel<object> response = await requestHandler.HandleAsync(
                bo,
                HttpContext
            );
            return Ok(response);
        }
        catch (Exception ex)
        {
            ActionsResponseModel<object> actionResponse = new();
            List<ErrorInfoModel> listError =
            [
                new ErrorInfoModel(
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
}
