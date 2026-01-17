using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletCounterpartyRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletCounterparty>(dataProvider, staticCacheManager),
        IWalletCounterpartyRepository
{
    public async Task<WalletCounterparty> FindByPhoneOrEmailAsync(int walletId, string phone, string email)
    {
        return await Table.Where(x =>
            x.WalletId == walletId &&
            (x.Phone == phone || x.Email == email))
            .FirstOrDefaultAsync();
    }
}
