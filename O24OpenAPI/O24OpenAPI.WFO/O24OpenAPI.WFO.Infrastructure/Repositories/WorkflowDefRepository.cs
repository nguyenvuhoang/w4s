using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WorkflowDefRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WorkflowDef>(dataProvider, staticCacheManager), IWorkflowDefRepository
{
    public async Task<WorkflowDef?> GetByWorkflowIdAndChannelIdAsync(string workflowId, string channelId)
    {
        return await Table.FirstOrDefaultAsync(x =>
            x.WorkflowId == workflowId && x.ChannelId == channelId
        );
    }
}
