using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;

namespace O24OpenAPI.ACT.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ForeignExchangeAccountDefinitionRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<ForeignExchangeAccountDefinition>(dataProvider, staticCacheManager),
        IForeignExchangeAccountDefinitionRepository
{
    public Task<ForeignExchangeAccountDefinition?> GetByCodeAsync(string fxCode) =>
        Table.FirstOrDefaultAsync(x => x.AccountCurrency == fxCode || x.ClearingCurrency == fxCode);
}
