using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories
{
    [RegisterService(Lifetime.Scoped)]
    public class WalletContractRepository(
        IO24OpenAPIDataProvider dataProvider,
        IStaticCacheManager staticCacheManager
    ) : EntityRepository<WalletContract>(dataProvider, staticCacheManager), IWalletContractRepository
    {
        public async Task<bool> ExistsByContractNumberAsync(string contractNumber)
        {
            var walletContract = await GetByContractNumberAsync(contractNumber);
            return walletContract != null;
        }

        public async Task<WalletContract> GetByContractNumberAsync(string contractNumber)
        {
            var contract = await Table.Where(x => x.ContractNumber == contractNumber)
                .FirstOrDefaultAsync();
            return contract;
        }

        public async Task<WalletContract> GetByPhoneAsync(string Phone)
        {
            var contract = await Table.Where(x => x.Phone == Phone)
                .FirstOrDefaultAsync();
            return contract;
        }
    }
}
