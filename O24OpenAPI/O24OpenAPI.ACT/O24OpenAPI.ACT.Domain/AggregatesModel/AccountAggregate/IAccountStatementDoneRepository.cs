using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

public interface IAccountStatementDoneRepository : IRepository<AccountStatementDone>
{
    Task<IReadOnlyList<AccountStatementDone>> GetByTransIdAsync(string transId);
}
