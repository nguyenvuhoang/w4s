using O24OpenAPI.Core;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Services.Interfaces;

namespace O24OpenAPI.WFO.Services;

public class WorkflowDefService(IRepository<WorkflowDef> workflowDefRepository)
    : IWorkflowDefService
{
    private readonly IRepository<WorkflowDef> _workflowDefRepository = workflowDefRepository;

    public virtual async Task<WorkflowDef> GetByIdAsync(int id)
    {
        return await _workflowDefRepository.GetById(id, cache => null);
    }

    public virtual async Task<WorkflowDef> GetByWorkflowIdAsync(
        string workflowId,
        string channelId
    )
    {
        return await _workflowDefRepository
            .Table.Where(s => s.WorkflowId == workflowId && s.ChannelId == channelId)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<IList<WorkflowDef>> GetAllAsync()
    {
        return await _workflowDefRepository.GetAll(query => query);
    }

    public async Task UpdateAsync(WorkflowDef workflowDef)
    {
        workflowDef.UpdatedOnUtc = DateTime.UtcNow;
        await _workflowDefRepository.Update(workflowDef);
    }

    public async Task<WorkflowDef> AddAsync(WorkflowDef workflowDef)
    {
        return await _workflowDefRepository.InsertAsync(workflowDef);
    }

    public async Task DeleteAsync(WorkflowDef workflowDef)
    {
        await _workflowDefRepository.Delete(workflowDef);
    }

    public async Task<IPagedList<WorkflowDef>> SimpleSearch(SimpleSearchModel model)
    {
        var query =
            from d in _workflowDefRepository.Table
            where
                string.IsNullOrEmpty(model.SearchText)
                || d.WorkflowId.Contains(model.SearchText)
            select d;
        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }
}
