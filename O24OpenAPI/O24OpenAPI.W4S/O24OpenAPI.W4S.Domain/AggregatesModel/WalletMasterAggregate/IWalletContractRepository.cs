using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate
{
    public interface IWalletContractRepository : IRepository<WalletContract>
    {
        /// <summary>
        /// Exists the by contract number asynchronous.
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <returns></returns>
        Task<bool> ExistsByContractNumberAsync(string contractNumber);
        /// <summary>
        /// Gets the by contract number asynchronous.
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <returns></returns>
        Task<WalletContract> GetByContractNumberAsync(string contractNumber);
    }
}
