using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Features.WorkflowLogs;

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
    public async Task<IActionResult> GetWorkflowLogByWorkflowId(
        [FromBody] GetWorkflowLogByExecutionIdQuery request
    )
    {
        GetWorkflowLogByExecutionIdResponse r = await mediator.QueryAsync(request);
        return Ok(r);
    }
}
