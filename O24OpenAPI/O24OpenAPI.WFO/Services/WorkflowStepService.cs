using O24OpenAPI.Core;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Services.Interfaces;

namespace O24OpenAPI.WFO.Services;

public class WorkflowStepService(IRepository<WorkflowStep> entityRepository)
    : IWorkflowStepService
{
    private readonly IRepository<WorkflowStep> _entityRepository = entityRepository;

    public virtual async Task<WorkflowStep> GetByIdAsync(int id)
    {
        return await _entityRepository.GetById(id, cache => null);
    }

    public virtual async Task<IList<WorkflowStep>> GetAllAsync()
    {
        return await _entityRepository.GetAll(query => query);
    }

    public async Task UpdateAsync(WorkflowStep entity)
    {
        await _entityRepository.Update(entity);
    }

    public async Task<WorkflowStep> AddAsync(WorkflowStep entity)
    {
        return await _entityRepository.InsertAsync(entity);
    }

    public async Task DeleteAsync(WorkflowStep entity)
    {
        await _entityRepository.Delete(entity);
    }

    public async Task<IPagedList<WorkflowStep>> SimpleSearch(SimpleSearchModel model)
    {
        var query =
            from d in _entityRepository.Table
            where
                (
                    !string.IsNullOrEmpty(model.SearchText)
                    && d.WorkflowId.Contains(model.SearchText)
                ) || true
            select d;
        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }

    public async Task DeleteByWorkflowIdAsync(string workflowId)
    {
        await _entityRepository.DeleteWhere(s => s.WorkflowId == workflowId);
    }

    public async Task<WorkflowStep> GetByWorkflowIdAndStepCode(string workflowId, string stepCode)
    {
        return await _entityRepository.Table.FirstOrDefaultAsync(e =>
            e.WorkflowId == workflowId && e.StepCode == stepCode
        );
    }
    public async Task<WorkflowStep> GetByWorkflowIdAndStepCodeAndStepOrder(string workflowId, string stepCode, int stepOrder)
    {
        return await _entityRepository.Table.FirstOrDefaultAsync(e =>
            e.WorkflowId == workflowId && e.StepCode == stepCode && e.StepOrder == stepOrder
        );
    }
    public async Task<IPagedList<WorkflowStep>> GetByWorkflowId(string workflowId)
    {
        var query =
            from d in _entityRepository.Table
            where
                (
                    !string.IsNullOrEmpty(workflowId)
                    && d.WorkflowId.Equals(workflowId)
                )
            select d;
        return await query.ToPagedList(0, 10);
    }
}
