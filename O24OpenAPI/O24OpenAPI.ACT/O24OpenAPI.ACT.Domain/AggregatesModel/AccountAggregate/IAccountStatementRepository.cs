using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

public interface IAccountStatementRepository : IRepository<AccountStatement>
{
    Task<IReadOnlyList<AccountStatement>> GetByTransIdAsync(string transId);
}
