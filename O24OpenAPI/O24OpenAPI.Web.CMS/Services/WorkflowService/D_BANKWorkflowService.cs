using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

/// <summary>
/// The bankworkflowservice class
/// </summary>
/// <seealso cref="BaseQueueService"/>

public class D_BANKWorkflowService : BaseQueueService
{
    /// <summary>
    /// The id bankservice
    /// </summary>
    private readonly ID_BANKService _dBank = EngineContext.Current.Resolve<ID_BANKService>();
    /// <summary>
    /// Gets the all using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the workflow scheme</returns>
    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(workflow, async ()=>
        {
            var list = await _dBank.GetAll();
            return list;
        });
    }
}
