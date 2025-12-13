using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class WorkflowDefinitionService(
    IRepository<WorkflowDefinition> workflowDefinitionRepo,
    IRepository<WorkflowStep> workflowStepRepo,
    IStaticCacheManager staticCacheManager
) : IWorkflowDefinitionService
{
    private readonly IRepository<WorkflowDefinition> _workflowDefinitionRepo =
        workflowDefinitionRepo;

    private readonly IRepository<WorkflowStep> _workflowStepRepo = workflowStepRepo;

    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    public async Task<WorkflowDefinition> GetByWFId(string wfId)
    {
        var cacheKey = new CacheKey($"{wfId}_DEF") { IsForever = true };
        return await _staticCacheManager.GetOrSetAsync(
            cacheKey,
            () => _workflowDefinitionRepo.Table.FirstOrDefaultAsync(e => e.WFId == wfId)
        );
    }

    public async Task<IPagedList<WorkflowDefinition>> SimpleSearch(SimpleSearchModel model)
    {
        var query = _workflowDefinitionRepo.Table;
        if (!string.IsNullOrEmpty(model.SearchText))
        {
            query = query.Where(e =>
                e.WFId.Contains(model.SearchText)
                || e.WFName.Contains(model.SearchText)
                || e.WFDescription.Contains(model.SearchText)
            );
        }

        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }

    public async Task<IPagedList<WorkflowDefinition>> AdvancedSearch(
        WorkflowDefinitionSearchModel model
    )
    {
        var query = _workflowDefinitionRepo.Table;
        if (model.WFId.HasValue())
        {
            query = query.Where(e => e.WFId == model.WFId);
        }

        if (model.WFName.HasValue())
        {
            query = query.Where(e => e.WFName == model.WFName);
        }

        if (model.WFDescription.HasValue())
        {
            query = query.Where(e => e.WFDescription == model.WFDescription);
        }

        return await query.ToPagedList(model.PageIndex, model.PageSize);
    }

    public async Task<List<string>> SeedDataWorkflowDef()
    {
        var existingWFIds = (
            await _workflowDefinitionRepo.Table.Select(d => d.WFId).ToListAsync()
        ).ToHashSet();

        var newWorkflows = await _workflowStepRepo
            .Table.GroupBy(step => step.WFId)
            .Select(group => new WorkflowDefinition
            {
                WFId = group.Key,
                WFName = group.First().WFId,
                WFDescription = group.First().Description,
                Status = WorkflowStatus.Active,
                TimeOut = 60000,
            })
            .Where(wf => !existingWFIds.Contains(wf.WFId))
            .ToListAsync();

        if (newWorkflows.HasValue())
        {
            // await _workflowDefinitionRepo.BulkInsert(newWorkflows);
            foreach (var item in newWorkflows)
            {
                await _workflowDefinitionRepo.Insert(item);
            }
        }

        return newWorkflows.Select(w => w.WFId).ToList();
    }
}
