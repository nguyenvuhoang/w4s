using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;

public interface IForeignExchangeAccountDefinitionRepository : IRepository<ForeignExchangeAccountDefinition>
{
    Task<ForeignExchangeAccountDefinition?> GetByCodeAsync(string fxCode);
}
