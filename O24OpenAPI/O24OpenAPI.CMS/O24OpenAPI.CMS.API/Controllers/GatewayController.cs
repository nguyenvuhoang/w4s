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
    [HttpPost("/api/channel/post")]
    public virtual async Task<IActionResult> Post([FromBody] BoRequestModel bo)
    {
        try
        {
            Dictionary<string, string> getAllCookies = Application.Utils.Utils.GetCookies(
                HttpContext
            );
            bool checkedSession = false;
            if (!getAllCookies.TryGetValue("device_id", out string value))
            {
                checkedSession = true;
            }
            else
            {
                string oldCookie = value.ToString();
                if (oldCookie != context.InfoRequest.DeviceID)
                {
                    checkedSession = true;
                }
            }

            if (checkedSession && !string.IsNullOrEmpty(context.InfoRequest.DeviceID))
            {
                HttpContext.Response.Cookies.Append(
                    "device_id",
                    context.InfoRequest.DeviceID,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true,
                        Expires = context?.InfoRequest.SessionExpired,
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

    [HttpPost("/api/v1/gateway")]
    public virtual async Task<IActionResult> ExecuteAsync([FromBody] RequestModel requestModel)
    {
        try
        {
            requestModel.Validate();
            Dictionary<string, string> getAllCookies = Application.Utils.Utils.GetCookies(
                HttpContext
            );
            bool checkedSession = false;
            if (!getAllCookies.TryGetValue("device_id", out string value))
            {
                checkedSession = true;
            }
            else
            {
                string oldCookie = value.ToString();
                if (oldCookie != context.InfoRequest.DeviceID)
                {
                    checkedSession = true;
                }
            }

            if (checkedSession && !string.IsNullOrEmpty(context.InfoRequest.DeviceID))
            {
                HttpContext.Response.Cookies.Append(
                    "device_id",
                    context.InfoRequest.DeviceID,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true,
                        Expires = context?.InfoRequest.SessionExpired,
                    }
                );
            }

            ResponseModel response = await requestHandler.ProcessAsync(requestModel, HttpContext);
            return Ok(response);
        }
        catch (Exception ex)
        {
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

            return Ok(ResponseFactory.Error(listError));
        }
    }
}
