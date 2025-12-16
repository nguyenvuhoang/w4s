using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services;

namespace O24OpenAPI.WFO.Controllers;

public class WorkflowController(
    IWorkflowExecutionService workflowExecutionService,
    IWorkflowService workflowService
) : BaseController
{
    private readonly IWorkflowExecutionService _workflowExecutionService =
        workflowExecutionService;
    private readonly IWorkflowService _workflowService = workflowService;

    [HttpPost]
    public async Task<IActionResult> Execute([FromBody] WorkflowInput request)
    {
        var r = await _workflowExecutionService.StartWorkflowAsync(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WorkflowInput request)
    {
        var r = await _workflowService.CreateWorkflow(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] WorkflowInput request)
    {
        var r = await _workflowService.SimpleSearch(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] WorkflowInput request)
    {
        var r = await _workflowService.UpdateWorkflow(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] WorkflowInput request)
    {
        var r = await _workflowService.DeleteWorkflow(request);
        return Ok(r);
    }
}
