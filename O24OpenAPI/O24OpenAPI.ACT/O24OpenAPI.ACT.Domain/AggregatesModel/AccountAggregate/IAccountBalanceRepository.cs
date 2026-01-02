using O24OpenAPI.ACT.Domain;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

public interface IAccountBalanceRepository : IRepository<AccountBalance>
{
    Task<AccountBalance?> GetByAccountNumberAsync(string accountNumber);
    Task<AccountBalance> GetByAccountNumber(string accountNumber);
}
