using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.ACT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class TransactionRulesRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<TransactionRules>(dataProvider, staticCacheManager),
        ITransactionRulesRepository
{
    public async Task<IReadOnlyList<TransactionRules>> GetByGroupAsync(string groupCode) =>
        await Table.Where(x => x.WorkflowId == groupCode).ToListAsync();
}
