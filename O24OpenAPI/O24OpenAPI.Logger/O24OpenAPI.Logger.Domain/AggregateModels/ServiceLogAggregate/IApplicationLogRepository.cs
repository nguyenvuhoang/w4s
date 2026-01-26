using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;

public interface IApplicationLogRepository : IRepository<ApplicationLog>
{
    Task ClearApplicationLogAsync();
}
