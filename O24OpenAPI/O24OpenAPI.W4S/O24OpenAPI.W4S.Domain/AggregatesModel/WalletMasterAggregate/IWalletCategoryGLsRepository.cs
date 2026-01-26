using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

public interface IWalletCategoryGLsRepository : IRepository<WalletCatalogGLs>
{
    public Task<WalletCatalogGLs> GetAccountingAccount(string catalogCode, string systaccountname);
}
