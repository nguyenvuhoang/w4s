using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate
{
    public interface IWalletCategoryDefaultRepository : IRepository<WalletCategoryDefault>
    {
        Task<IList<WalletCategoryDefault>> GetActiveAsync(string language = null);
    }
}
