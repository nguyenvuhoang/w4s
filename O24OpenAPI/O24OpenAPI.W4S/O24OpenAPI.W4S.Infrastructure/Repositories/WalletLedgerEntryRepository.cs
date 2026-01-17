using LinKit.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletLedgerEntryRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletLedgerEntry>(dataProvider, staticCacheManager), IWalletLedgerEntryRepository
{
    public async Task<WalletLedgerEntry> AddAsync(WalletLedgerEntry entity)
    {
        return await InsertAsync(entity);
    }

    public async Task PostingEntry(List<WalletLedgerEntry> entity)
    {
        await BulkInsert(entity);
    }
}
