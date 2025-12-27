using LinqToDB;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class WorkflowStepService(IRepository<WorkflowStep> workflowStepRepo)
    : IWorkflowStepService
{
    private readonly IRepository<WorkflowStep> _workflowStepRepo = workflowStepRepo;

    public async Task<IPagedList<WorkflowStep>> AdvancedSearch(WorkflowStepSearchModel model)
    {
        await _workflowStepService.DeleteByWorkflowIdAsync(wfDef.WorkflowId);

        var query = _workflowStepRepo.Table;
        if (!string.IsNullOrEmpty(model.WFId))
        {
            query = query.Where(e => e.WFId == model.WFId);
        }

        if (!string.IsNullOrEmpty(model.StepCode))
        {
            query = query.Where(e => e.StepCode == model.StepCode);
        }

        if (!string.IsNullOrEmpty(model.Description))
        {
            query = query.Where(e => e.Description == model.Description);
        }

        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }

    public async Task<WorkflowStep> GetByWorkflowIdAndStep(string workflowId, string stepCode)
    {
        return await _workflowStepRepo.Table.FirstOrDefaultAsync(e =>
            e.WFId == workflowId && e.StepCode == stepCode && e.Status
        );
    }

    public async Task<List<WorkflowStep>> GetStepsByWorkflowIdAsync(
        string workflowId,
        string appCode
    )
    {
        var q = _workflowStepRepo.Table.Where(s => s.WFId == workflowId && s.Status);
        if (!string.IsNullOrEmpty(appCode))
        {
            q = q.Where(s => s.AppCode == appCode);
        }
        return await q.OrderBy(e => e.StepOrder).ToListAsync();
    }

    public async Task<IPagedList<WorkflowStep>> SimpleSearch(SimpleSearchModel model)
    {
        var query = _workflowStepRepo.Table;
        if (!string.IsNullOrEmpty(model.SearchText))
        {
            query = query.Where(e =>
                e.StepCode.Contains(model.SearchText)
                || e.Description.Contains(model.SearchText)
                || e.WFId.Contains(model.SearchText)
            );
        }

        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }
}
