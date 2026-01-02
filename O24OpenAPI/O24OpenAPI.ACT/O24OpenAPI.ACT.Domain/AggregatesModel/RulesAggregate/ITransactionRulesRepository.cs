using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;

public interface ITransactionRulesRepository : IRepository<TransactionRules>
{
    Task<IReadOnlyList<TransactionRules>> GetByGroupAsync(string groupCode);
}
