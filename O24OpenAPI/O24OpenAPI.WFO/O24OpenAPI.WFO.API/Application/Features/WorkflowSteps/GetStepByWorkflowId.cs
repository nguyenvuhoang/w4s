using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Models.WorkflowStepModels;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Features.WorkflowSteps;

public class GetStepByWorkflowIdQuery : BaseSearch, IQuery<IPagedList<WorkflowStepModel>>
{
    public string WorkflowId { get; set; }
}

[CqrsHandler]
public class GetStepByWorkflowIdHandler(IWorkflowStepRepository workflowStepRepository)
    : IQueryHandler<GetStepByWorkflowIdQuery, IPagedList<WorkflowStepModel>>
{
    public async Task<IPagedList<WorkflowStepModel>> HandleAsync(
        GetStepByWorkflowIdQuery request,
        CancellationToken cancellationToken = default
    )
    {
        List<WorkflowStep> listStep = await workflowStepRepository
            .Table.Where(s => s.WorkflowId == request.WorkflowId)
            .ToListAsync();

        List<WorkflowStepModel> listStepModel = listStep.ToWorkflowStepModelList();
        return await listStepModel.AsQueryable().ToPagedList(request.PageIndex, request.PageSize);
    }
}
