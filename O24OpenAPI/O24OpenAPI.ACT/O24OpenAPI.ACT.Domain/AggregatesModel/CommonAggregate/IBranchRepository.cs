using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;

public interface IBranchRepository : IRepository<Branch>
{
    Task<Branch?> GetByCodeAsync(string branchCode);
}
