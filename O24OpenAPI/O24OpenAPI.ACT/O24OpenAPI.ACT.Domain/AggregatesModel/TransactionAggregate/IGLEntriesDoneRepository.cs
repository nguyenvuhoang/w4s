using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;

public interface IGLEntriesDoneRepository : IRepository<GLEntriesDone>
{
    Task<IReadOnlyList<GLEntriesDone>> GetByTransIdAsync(string transId);
}
