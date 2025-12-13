using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.CMS.Models.Neptune;
using O24OpenAPI.Web.CMS.Services.NeptuneService;
using O24OpenAPI.Web.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers;

public class ExecuteNeptuneWorkflowController(INeptuneCBSService neptuneCBSService)
    : BaseController
{
    private readonly INeptuneCBSService _neptuneCBSService = neptuneCBSService;

    [HttpPost]
    public virtual async Task<IActionResult> ExecuteWorkflow(
        [FromBody] ExecuteWorkflowNeptuneModel model
    )
    {
        var response = await _neptuneCBSService.ExecuteWorkflow(model);
        return Ok(response);
    }
}
