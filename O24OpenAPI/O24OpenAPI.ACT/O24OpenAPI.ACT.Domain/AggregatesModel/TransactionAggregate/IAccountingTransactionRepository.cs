using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.TransactionAggregate;

public interface IAccountingTransactionRepository : IRepository<AccountingTransaction>
{
    Task<IReadOnlyList<AccountingTransaction>> GetByRefAsync(string referenceId);
}
