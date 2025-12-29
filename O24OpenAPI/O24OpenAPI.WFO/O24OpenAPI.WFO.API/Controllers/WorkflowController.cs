using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Features.WorkflowExecutions;
using O24OpenAPI.WFO.API.Application.Features.Workflows;
using WorkflowInput = O24OpenAPI.WFO.API.Application.Models.WorkflowInput;

namespace O24OpenAPI.WFO.API.Controllers;

public class WorkflowController(
    IWorkflowExecutionHandler workflowExecutionService,
    ICreateWorkflowHandler createWorkflowHandler,
    ISearchWorkflowHandler searchWorkflowHandler,
    IUpdateWorkflowHandler updateWorkflowHandler,
    IDeleteWorkflowHandler deleteWorkflowHandler
) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Execute([FromBody] WorkflowInput request)
    {
        WorkflowResponse r = await workflowExecutionService.StartWorkflowAsync(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WorkflowInput request)
    {
        WorkflowResponse r = await createWorkflowHandler.CreateWorkflow(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SimpleSearchModel request)
    {
        WorkflowResponse r = await searchWorkflowHandler.SimpleSearch(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] WorkflowInput request)
    {
        WorkflowResponse r = await updateWorkflowHandler.UpdateWorkflow(request);
        return Ok(r);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] WorkflowInput request)
    {
        WorkflowResponse r = await deleteWorkflowHandler.DeleteWorkflow(request);
        return Ok(r);
    }
}
