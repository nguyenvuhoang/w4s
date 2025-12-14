using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Services.Interfaces;

namespace O24OpenAPI.WFO.Services;

public class WorkflowInfoService(IRepository<WorkflowInfo> wfInfoRepo) : IWorkflowInfoService
{
    private readonly IRepository<WorkflowInfo> _wfInfoRepo = wfInfoRepo;

    public async Task AddAsync(WorkflowInfo workflowInfo)
    {
        await _wfInfoRepo.Insert(workflowInfo);
    }

    public async Task UpdateAsync(WorkflowInfo workflowInfo)
    {
        await _wfInfoRepo.Update(workflowInfo);
    }
}
