using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Features.WorkflowLogs;
using O24OpenAPI.WFO.API.Application.Models;

namespace O24OpenAPI.WFO.API.Controllers;

public class WorkflowLogController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AdvancedSearchWorkflowLog(
        [FromBody] AdvancedSearchWorkflowLogQuery request
    )
    {
        PagedListModel<AdvancedSearchWorkflowLogResponse> r = await mediator.QueryAsync(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> GetWorkflowStepLogByWorkflowId(
        [FromBody] GetWorkflowStepLogByExecutionIdQuery request
    )
    {
        PagedListModel<WorkflowStepInfoModel> r = await mediator.QueryAsync(request);
        return Ok(r);
    }
}
