using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletTransactionRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletTransaction>(dataProvider, staticCacheManager),
        IWalletTransactionRepository
{
    public async Task<IList<WalletTransaction>> GetByWalletIdAsync(Guid walletId, DateTime? fromUtc = null, DateTime? toUtc = null, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
    {
        var wid = walletId.ToString();

        var q = Table.Where(x => x.SourceTranRef == wid);

        if (fromUtc.HasValue)
            q = q.Where(x => x.TransactionDate >= fromUtc.Value);

        if (toUtc.HasValue)
            q = q.Where(x => x.TransactionDate < toUtc.Value);

        return (IList<WalletTransaction>)await q
            .OrderByDescending(x => x.TransactionDate)
            .ThenByDescending(x => x.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}
