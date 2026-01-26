using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

public interface IAccountCommonRepository : IRepository<AccountCommon>
{
    Task<AccountCommon?> GetByAccountNumberAsync(string accountNumber);
}
