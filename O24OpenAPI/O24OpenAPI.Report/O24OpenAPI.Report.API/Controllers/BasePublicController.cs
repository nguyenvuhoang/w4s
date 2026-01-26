using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Report.API.Application.Models.Mvc;
using System.Diagnostics;

namespace O24OpenAPI.Report.API.Controllers;

[ProducesResponseType(typeof(string), 401)]
[ApiController]
[Produces("application/json", new string[] { })]
[Route("[controller]/[action]", Order = 2147483647)]
public abstract partial class BasePublicController : Controller
{
    protected virtual IActionResult InvokeHttp404(string message)
    {
        Response.StatusCode = 404;
        return View(
            "Error",
            new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message,
                ErrorCode = 404,
            }
        );
    }

    protected virtual IActionResult InvokeHttp401()
    {
        Response.StatusCode = 401;
        return View(
            "Error",
            new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "Invalid Session",
                ErrorCode = 401,
            }
        );
    }
    protected virtual IActionResult InvokeHttp500(string message)
    {
        Response.StatusCode = 500;
        return View(
            "Error",
            new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message,
                ErrorCode = 500,
            }
        );
    }

    protected virtual IActionResult InvokeHttp400(string message)
    {
        Response.StatusCode = 400;
        return View(
            "Error",
            new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message,
                ErrorCode = 400,
            }
        );
    }

}
