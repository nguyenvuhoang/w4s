using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.O24OpenAPIClient.Enums;
using O24OpenAPI.Web.Framework.Controllers;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.O24NCH.Controllers;

public partial class MailChannelController : BaseController
{

    [HttpPost]
    public virtual async Task<IActionResult> Post(
        [FromBody] CrossServiceRequest crossServiceRequest
    )
    {
        if (crossServiceRequest.ProcessNumber != ProcessNumber.ExecuteCommand)
        {
            return Ok(
                await BaseQueueService.InvokeCommandQuery(crossServiceRequest.WorkflowScheme)
            );
        }
        var response = await BaseQueueService.InvokeAsync(
            crossServiceRequest.WorkflowScheme,
            crossServiceRequest.FullClassName,
            crossServiceRequest.MethodName,
            "O24OpenAPI.O24NCH"
        );
        return Ok(response);
    }
}
