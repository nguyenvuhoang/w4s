using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

public interface IWalletAccountGLsRepository : IRepository<WalletAccountGLs>
{
    Task<WalletAccountGLs> GetAccountingAccount(string accountnumber, string accounttype);
}
