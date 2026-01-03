using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.ACT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class BranchRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<Branch>(dataProvider, staticCacheManager), IBranchRepository
{
    public Task<Branch?> GetByCodeAsync(string branchCode) =>
        Table.FirstOrDefaultAsync(x => x.BranchCode == branchCode);
}
