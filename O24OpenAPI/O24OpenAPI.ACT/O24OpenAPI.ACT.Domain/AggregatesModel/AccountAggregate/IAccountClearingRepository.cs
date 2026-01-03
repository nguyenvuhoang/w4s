using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

public interface IAccountClearingRepository : IRepository<AccountClearing>
{
    Task<IReadOnlyList<AccountClearing>> GetByAccountNumberAsync(string accountNumber);
}
