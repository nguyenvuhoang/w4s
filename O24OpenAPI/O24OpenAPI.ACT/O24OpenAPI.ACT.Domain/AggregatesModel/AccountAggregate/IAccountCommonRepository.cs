using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.ACT.Domain;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

public interface IAccountCommonRepository : IRepository<AccountCommon>
{
    Task<AccountCommon?> GetByAccountNumberAsync(string accountNumber);
}
