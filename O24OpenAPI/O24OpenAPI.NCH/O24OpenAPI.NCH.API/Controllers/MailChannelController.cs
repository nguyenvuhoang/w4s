using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Client.Enums;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.NCH.API.Controllers;

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
            "O24OpenAPI.NCH"
        );
        return Ok(response);
    }
}
