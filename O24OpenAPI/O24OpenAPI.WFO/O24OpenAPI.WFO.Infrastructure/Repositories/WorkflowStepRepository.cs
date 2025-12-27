using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WorkflowStepRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WorkflowStep>(dataProvider, staticCacheManager), IWorkflowStepRepository
{
    public async Task<List<WorkflowStep>> GetByWorkflowIdAsync(string workflowId)
    {
        return await Table
            .Where(x => x.WorkflowId == workflowId && x.Status)
            .OrderBy(x => x.StepOrder)
            .ToListAsync();
    }

    public async Task DeleteByWorkflowIdAsync(string workflowId)
    {
        await DeleteWhere(s => s.WorkflowId == workflowId);
    }

    public async Task<WorkflowStep?> GetByWorkflowIdAndStepCode(string workflowId, string stepCode)
    {
        return await Table.FirstOrDefaultAsync(e =>
            e.WorkflowId == workflowId && e.StepCode == stepCode
        );
    }

    public async Task<IPagedList<WorkflowStep>> SimpleSearch(SimpleSearchModel model)
    {
        IQueryable<WorkflowStep> query =
            from d in Table
            where
                (!string.IsNullOrEmpty(model.SearchText) && d.WorkflowId.Contains(model.SearchText))
                || true
            select d;
        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }

    public async Task<IPagedList<WorkflowStep>> GetByWorkflowId(string workflowId)
    {
        IQueryable<WorkflowStep> query =
            from d in Table
            where !string.IsNullOrEmpty(workflowId) && d.WorkflowId.Equals(workflowId)
            select d;
        return await query.ToPagedList(0, 10);
    }

    public async Task<WorkflowStep> GetByWorkflowIdAndStepCodeAndStepOrder(
        string workflowId,
        string stepCode,
        int stepOrder
    )
    {
        return await Table.FirstOrDefaultAsync(e =>
            e.WorkflowId == workflowId && e.StepCode == stepCode && e.StepOrder == stepOrder
        );
    }
}
