using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class ExecuteQueryQueue : BaseQueueService
{
    public async Task<WorkflowScheme> ExecuteSql(WorkflowScheme workflow)
    {
        return await InvokeCommandQuery(workflow);
    }

    public async Task<WorkflowScheme> ExecuteAction(WorkflowScheme workflow)
    {
        return await InvokeCommandDML(workflow);
    }
}
